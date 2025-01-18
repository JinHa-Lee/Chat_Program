using ServerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server
{
    struct JobTimerElem : IComparable<JobTimerElem>
    {
        // sort, 비교 할때 IComparable 인터페이스를 사용

        public int execTick; // 실행 시간
        public Action action; // 실행할 기능

        public int CompareTo(JobTimerElem other) // 실행 시간 비교
        {
            return other.execTick - this.execTick; // 상대방의 실행시간 - 나의 실행시간
        }
    }

    internal class JobTimer
    {
        PriorityQueue<JobTimerElem> _pq = new PriorityQueue<JobTimerElem>();
        object _lock = new object();

        public static JobTimer Instance { get; } = new JobTimer(); // instance로 바로접근 가능

        public void Push(Action action, int tickAfter = 0) // tickAfter = 몇틱후에 실행할 것인지
        {
            JobTimerElem job;
            job.execTick = System.Environment.TickCount + tickAfter; // 실행할 시각
            job.action = action; // 실행할 기능

            lock (_lock)
            {
                _pq.Push(job);
            }
        }

        public void Flush()
        {
            while (true)
            {
                int now =System.Environment.TickCount;

                JobTimerElem job;

                lock (_lock)
                {
                    if (_pq.Count == 0)
                        break; // lock을 빠져나감

                    job = _pq.Peek(); // 다음 실행할 기능 확인
                    if (job.execTick > now) // 내가 실행해야하는 시간 > now 즉, 실행시간이 남았다
                        break;

                    //실행해야 한다면
                    _pq.Pop(); // 뽑아서 없앰
                }

                job.action.Invoke(); // 기능 실행
            }
        }

    }
}
