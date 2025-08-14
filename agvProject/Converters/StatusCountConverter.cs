using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using agvProject.Models;

namespace agvProject.Converters
{
    public class StatusCountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is System.Collections.ICollection missions && parameter is string status)
            {
                var missionCollection = missions.Cast<Mission>();
                
                switch (status)
                {
                    case "완료":
                        return missionCollection.Count(m => m.Status == MissionStatus.Completed);
                    case "진행중":
                        return missionCollection.Count(m => m.Status == MissionStatus.Processing);
                    case "대기":
                        return missionCollection.Count(m => m.Status == MissionStatus.Standby);
                    case "오류":
                        return missionCollection.Count(m => m.Status == MissionStatus.Error);
                    case "취소":
                        return missionCollection.Count(m => m.Status == MissionStatus.Canceled);
                    default:
                        return 0;
                }
            }
            
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

