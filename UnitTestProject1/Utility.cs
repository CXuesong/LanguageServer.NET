using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject1
{
    class Utility
    {

        /// <summary>
        /// Runs Task synchronously, and returns the result.
        /// </summary>
        public static T AwaitSync<T>(Task<T> task)
        {
            if (task == null) throw new ArgumentNullException(nameof(task));
            return task.GetAwaiter().GetResult();
        }

        /// <summary>
        /// Runs Task synchronously, and returns the result.
        /// </summary>
        public static void AwaitSync(Task task)
        {
            if (task == null) throw new ArgumentNullException(nameof(task));
            task.GetAwaiter().GetResult();
        }

        public static Stream StringToStream(string str)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));
            var buffer = Encoding.UTF8.GetBytes(str);
            return new MemoryStream(buffer);
        }

    }
}
