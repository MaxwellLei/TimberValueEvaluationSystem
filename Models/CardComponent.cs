using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TimberValueEvaluationSystem.Models
{
    public class CardComponent
    {
        public string Content { get; set; }

        public void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            MouseDown?.Invoke(sender, e);
        }

        public void OnMouseMove(object sender, MouseEventArgs e)
        {
            MouseMove?.Invoke(sender, e);
        }

        public void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            MouseUp?.Invoke(sender, e);
        }

        public event MouseButtonEventHandler MouseDown;
        public event MouseEventHandler MouseMove;
        public event MouseButtonEventHandler MouseUp;
    }
}
