using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Control;

namespace Library.Interface
{
    public interface IStationControl
    {
        StationControl.LadeskabState _state { get; set; }
        int _oldID { get;}

    }
}
