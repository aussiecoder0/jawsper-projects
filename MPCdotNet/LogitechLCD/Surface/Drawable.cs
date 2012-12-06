using System.Drawing;
namespace LogitechLCD
{
    public abstract class Drawable
    {
        protected bool m_Changed = true;
        protected Point m_Position;

        public Drawable(Point a_Position)
        {
            m_Position = a_Position;
        }

        public abstract bool Draw(Surface a_Surface);
    }
}