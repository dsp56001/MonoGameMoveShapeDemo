using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WebShapeConsoleClient
{
    public class ShapeModel : INotifyPropertyChanged
    {
        public bool Updated;
        // We declare Left and Top as lowercase with 
        // JsonProperty to sync the client and server models
        double left;
        public double Left
        {
            get { return this.left; }
            set
            {
                if (value != this.left)
                {
                    this.left = value;
                    NotifyPropertyChanged("Left");
                }
            }
        }

        double top;
        public double Top
        {
            get { return this.top; }
            set
            {
                if (value != this.top)
                {
                    this.top = value;
                    NotifyPropertyChanged("Top");
                }
            }
        } 
        

        public override string ToString()
        {
            return string.Format("{0}:{1}", Left, Top);
        }


        // boiler-plate
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
