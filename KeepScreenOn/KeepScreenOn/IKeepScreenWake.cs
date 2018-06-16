using System;
using System.Collections.Generic;
using System.Text;

namespace KeepScreenOn
{
    public interface IKeepScreenWake
    {
		void KeepScreenOn();

		bool IsActive();
    }
}
