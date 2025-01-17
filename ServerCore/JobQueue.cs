using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerCore
{
    public interface IJobQueue
    {
        void Push(Action job);
    }

    public class JobQueue : IJobQueue
    {
        Queue<Action> _jobQueue = new Queue<Action>();
        object _lock = new object();
        // 큐에 쌓인 기능을 실행시킬때 사용
        bool _flush = false;

        public void Push(Action job)
        {
            bool flush = false;
            lock (_lock)
            {
                _jobQueue.Enqueue(job);
                if (_flush == false)
                {
                    // 큐에 데이터가 적재되었기때문에 실행을 위해 true로 변경
                    flush = _flush = true;
                }
            }
            if (flush)
                Flush();
        }

        Action Pop()
        {
            lock (_lock)
            {
                if (_jobQueue.Count == 0)
                {
                    _flush = false;
                    return null;
                }
                return _jobQueue.Dequeue();
            }
        }
        void Flush()
        {
            while (true)
            {
                Action action = Pop();
                if (action == null)
                    return;
                action.Invoke();
            }
        }
    }
}
