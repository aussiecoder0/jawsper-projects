using System;
using System.Collections.Generic;

namespace MPCdotNet
{
    public partial class MPC
    {
        public event EventHandler OnConnectionStateChanged;

        public bool Connected { get; private set; }

        public Status PreviousStatus { get; private set; }
        public Status CurrentStatus { get; private set; }
        public Stats CurrentStats { get; private set; }
        public List<Output> Outputs { get; private set; }
        public SongInfo CurrentSong { get; private set; }
        public Playlist CurrentPlaylist { get; private set; }

        private ServerComponent s;

        public MPC(Server server) : this(server.Host, server.Port)
        {
        }
        public MPC(string host, int port)
        {
            s = new ServerComponent(host, port);
            CurrentStatus = new Status();
            CurrentPlaylist = new Playlist();
        }

        public void Open()
        {
            if (Connected) return;

            CurrentStatus = null;
            CurrentStats = null;
            Outputs = null;
            CurrentSong = null;
            CurrentPlaylist.Clear();

            Connected = s.Open();
            UpdateOutputs();
            if (OnConnectionStateChanged != null) OnConnectionStateChanged(this, null);
        }

        public void Close()
        {
            if (!Connected) return;

            s.Close();

            Connected = false;
            CurrentStatus = new Status();
            CurrentStats = null;
            Outputs = null;
            CurrentSong = new SongInfo();
            CurrentPlaylist.Clear();

            if (OnConnectionStateChanged != null) OnConnectionStateChanged(this, null);
        }

        public bool Update()
        {
            if (Connected)
            {
                PreviousStatus = CurrentStatus;
                CurrentStatus = new Status(s.SendCommand("status"));
                if (PreviousStatus != null && PreviousStatus.PlaylistVersion != CurrentStatus.PlaylistVersion)
                {
                    UpdateCurrentPlaylist();
                }
                return true;
            }
            return false;
        }

        public void UpdateStats()
        {
            CurrentStats = new Stats(s.SendCommand("stats"));
        }
        private void UpdateOutputs()
        {
            Outputs = new List<Output>();

            var list = s.SendCommand("outputs");
            int id = 0; string name = "";
            foreach (var kv in list)
            {
                if (kv.Key == "outputid") id = int.Parse(kv.Value);
                else if (kv.Key == "outputname") name = kv.Value;
                else
                {
                    Outputs.Add(new Output(id, name, kv.Value == "1"));
                }
            }
        }

        public void UpdateCurrentSongInfo()
        {
            CurrentSong = GetCurrentSong();
        }

        public void UpdateCurrentPlaylist()
        {
            List<KeyValuePair<string, string>> result;
            if (PreviousStatus == null)
            {
                result = s.SendCommand("playlistinfo");
            }
            else
            {
                Console.WriteLine("Playlist updated! old_version is " + PreviousStatus.PlaylistVersion);
                Console.WriteLine("Length diff: {0} -> {1}", PreviousStatus.PlaylistLength, CurrentStatus.PlaylistLength);
                //result = SendCommand("plchanges", old_status.Playlist.ToString());
                int new_pos = -1;
                foreach (var item in s.SendCommand("plchangesposid", PreviousStatus.PlaylistVersion.ToString()))
                {
                    if (item.Key == "cpos") new_pos = int.Parse(item.Value);
                    else
                    {
                        int id = int.Parse(item.Value);
                        int old_pos = -1;
                        for (int i = 0; i < CurrentPlaylist.Count; i++)
                        {
                            if (CurrentPlaylist[i].ID == id)
                            {
                                old_pos = i;
                                var tmp = CurrentPlaylist[new_pos];
                                CurrentPlaylist[new_pos] = CurrentPlaylist[old_pos];
                                CurrentPlaylist[old_pos] = tmp;
                                break;
                            }
                        }
                        if (old_pos == -1)
                        {
                            var data = s.SendCommand("playlistinfo", new_pos.ToString());
                            CurrentPlaylist.Insert(new_pos, new PlaylistEntry(data));
                        }
                        Console.WriteLine("Item: old_pos: {0}; cpos: {1}; id: {2}", old_pos, new_pos, id);
                    }
                }


                if (CurrentStatus.PlaylistLength < PreviousStatus.PlaylistLength)
                {
                    while (CurrentPlaylist.Count > CurrentStatus.PlaylistLength)
                    {
                        CurrentPlaylist.RemoveAt(CurrentPlaylist.Count - 1);
                    }
                    Console.WriteLine("Remove end");
                }
                result = null;
            }

            if (result != null)
            {
                PlaylistEntry entry;
                var buffer = new List<KeyValuePair<string, string>>();
                foreach (var item in result)
                {
                    if (buffer.Count > 0 && item.Key == "file")
                    {
                        entry = new PlaylistEntry(buffer);
                        if (entry.Pos >= 0)
                            CurrentPlaylist.Insert(entry.Pos, entry);
                        else
                            CurrentPlaylist.Add(entry);
                        buffer.Clear();
                    }
                    buffer.Add(item);
                }
                if (buffer.Count > 0)
                {
                    entry = new PlaylistEntry(buffer);
                    if (entry.Pos >= 0)
                        CurrentPlaylist.Insert(entry.Pos, entry);
                    else
                        CurrentPlaylist.Add(entry);
                }
            }
        }

        public List<KeyValuePair<string,string>> GetCommands()
        {
            return s.SendCommand("commands");
        }
        public List<KeyValuePair<string, string>> GetNotCommands()
        {
            return s.SendCommand("notcommands");
        }

        public SongInfo GetCurrentSong()
        {
            return new SongInfo(s.SendCommand("currentsong"));
        }

        public void SendPlaybackCommand(PlaybackCommand key, string args = "" )
        {
            if (!Connected) return;
            switch (key)
            {
                case PlaybackCommand.Pause:
                    s.SendCommand("pause", args);
                    break;
                case PlaybackCommand.PlayPause:
                    if (CurrentStatus == null || CurrentStatus.PlaybackState == PlaybackState.Stopped)
                    {
                        s.SendCommand("play");
                    }
                    else
                    {
                        switch (CurrentStatus.PlaybackState)
                        {
                            case PlaybackState.Playing:
                                s.SendCommand("pause", "1");
                                break;
                            case PlaybackState.Paused:
                                s.SendCommand("pause", "0");
                                break;
                        }
                    }
                    break;
                case PlaybackCommand.Stop:
                    s.SendCommand("stop");
                    break;

                case PlaybackCommand.PreviousTrack:
                    s.SendCommand("previous");
                    break;
                case PlaybackCommand.NextTrack:
                    s.SendCommand("next");
                    break;

                case PlaybackCommand.Play:
                    s.SendCommand("play", args);
                    break;
                case PlaybackCommand.PlayID:
                    s.SendCommand("playid", args);
                    break;
                case PlaybackCommand.Seek:
                    s.SendCommand("seek", args);
                    break;
                case PlaybackCommand.SeekID:
                    s.SendCommand("seekid", args);
                    break;
                case PlaybackCommand.SeekCurr:
                    s.SendCommand("seekcurr", args);
                    break;
            }
        }
        private void SetBool(string command, bool state)
        {
            s.SendCommand(command, state ? "1" : "0");
        }
        public void SetRepeat( bool state)
        {
            SetBool("repeat", state);
        }
        public void SetRandom( bool state )
        {
            SetBool("random", state);
        }
        public void SetSingle(bool state)
        {
            SetBool("single", state);
        }
        public void SetConsume(bool state)
        {
            SetBool("consume", state);
        }

        public void SetOutput(int id, bool state)
        {
            s.SendCommand(state ? "enableoutput" : "disableoutput", id.ToString());
        }

        public SongInfo GetSongInfoBySongID(int SongID)
        {
            return null;
        }

        public void SeekCurrent(int time)
        {
            // s.SendCommand( "seekcur", time.ToString());
            Seek(CurrentSong.ID, time);
        }
        public void Seek(int songid, int time)
        {
            s.SendCommand("seekid", songid.ToString(), time.ToString());
        }

        public List<KeyValuePair<string, string>> SendRawCommand(string command, params string[] args)
        {
            return s.SendCommand(command, args);
        }
    }
    public enum PlaybackCommand
    {
        Pause,
        PlayPause,
        Stop,
        PreviousTrack,
        NextTrack,

        Play, PlayID,
        Seek, SeekID, SeekCurr
    }
    public enum PlaybackState { Undefined, Stopped, Playing, Paused }
}
