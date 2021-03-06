﻿using System.Collections.Generic;
using System.Linq;

using Android.App;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Java.Lang;
using Java.Util;
using KeepScreenOn.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(KeepScreenWake))]
namespace KeepScreenOn.Droid
{
	public class KeepScreenWake : Java.Lang.Object, IKeepScreenWake
	{
		private static Activity GetCurrentActivity()
		{
			Activity activity = null;
			List<Java.Lang.Object> objects = null;

			var activityThreadClass = Class.ForName("android.app.ActivityThread");
			var activityThread = activityThreadClass.GetMethod("currentActivityThread").Invoke(null);
			var activityFields = activityThreadClass.GetDeclaredField("mActivities");
			activityFields.Accessible = true;

			var obj = activityFields.Get(activityThread);

			if (obj is JavaDictionary)
			{
				var activities = (JavaDictionary)obj;
				objects = new List<Java.Lang.Object>(activities.Values.Cast<Java.Lang.Object>().ToList());
			}
			else if (obj is ArrayMap)
			{
				var activities = (ArrayMap)obj;
				objects = new List<Java.Lang.Object>(activities.Values().Cast<Java.Lang.Object>().ToList());
			}
			else if (obj is IMap)
			{
				var activities = (IMap)activityFields.Get(activityThread);
				objects = new List<Java.Lang.Object>(activities.Values().Cast<Java.Lang.Object>().ToList());
			}

			if (objects != null && objects.Any())
			{
				foreach (var activityRecord in objects)
				{
					var activityRecordClass = activityRecord.Class;
					var pausedField = activityRecordClass.GetDeclaredField("paused");
					pausedField.Accessible = true;

					if (!pausedField.GetBoolean(activityRecord))
					{
						var activityField = activityRecordClass.GetDeclaredField("activity");
						activityField.Accessible = true;
						activity = (Activity)activityField.Get(activityRecord);
						break;
					}
				}
			}

			return activity;
		}

		public bool IsActive()
		{
			var activity = GetCurrentActivity();
			return activity.Window.Attributes.Flags.HasFlag(WindowManagerFlags.KeepScreenOn);
		}

		public void KeepScreenOn()
		{
			var activity = GetCurrentActivity();

			activity.Window.SetFlags(WindowManagerFlags.KeepScreenOn, WindowManagerFlags.KeepScreenOn);
		}
	}
}