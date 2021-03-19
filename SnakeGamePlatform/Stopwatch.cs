using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGamePlatform
{
    class Stopwatch
    {
        private DateTime _start;
        private DateTime _stop;
        private bool _running;

        public void Start()
        {
            if (!_running)
            {
                _running = true;
                _start = DateTime.Now;
            }
        }
        public TimeSpan Stop()
        {
            if (_running)
            {
                _stop = DateTime.Now;
                _running = false;
            }
            return (_stop - _start);
        }
    }
}
