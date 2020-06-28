using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace MongoPie
{
    public class GlobalExceptionHelper
    {
        /// <summary>
        /// 全局报错展示异常
        /// </summary>
        /// <param name="exception">异常</param>
        /// <param name="Message">消息</param>
        /// <param name="IsDetail">是否展示堆栈</param>
        public static void Display(Exception exception, string Message = "", bool ShowDetail = false)
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Message))
            {
                builder.AppendLine(Message);
                builder.AppendLine(string.Empty);
            }

            if(exception != null)
            {
                builder.AppendLine(exception.Message);

                if (ShowDetail)
                {
                    builder.Append(exception.StackTrace);
                }

            }

            if (!string.IsNullOrEmpty(builder.ToString()))
            {
                MessageBox.Show(App.Current.MainWindow, builder.ToString(), "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
