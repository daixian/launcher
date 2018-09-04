using System;
using System.Collections.Generic;


namespace xuexue
{
    /// <summary>
    /// 一个线程同步队列
    /// </summary>
    public class DQueue<T>
    {
        /// <summary>
        /// 构造函数，参数是队列的最大长度。
        /// 如果调用EnqueueMaxLimit()当队列长度要超过的时候会自动丢弃最前端的内容。
        /// </summary>
        /// <param name="maxCapacity">队列的最大长度</param>
        public DQueue(int maxCapacity)
        {
            this._queue = new Queue<T>(maxCapacity);//直接申请最大容量
            maxCount = maxCapacity;
        }

        /// <summary>
        /// 构造函数，参数是队列的最大长度，和初始长度。
        /// 如果调用EnqueueMaxLimit()当队列长度要超过的时候会自动丢弃最前端的内容。
        /// </summary>
        /// <param name="maxCapacity">队列的最大长度</param>
        /// <param name="initSize">初始分配长度</param>
        public DQueue(int maxCapacity, int initSize)
        {
            this._queue = new Queue<T>(initSize);
            maxCount = maxCapacity;
        }

        private Queue<T> _queue;

        /// <summary> 队列的最大数量. </summary>
        private int maxCount;

        /// <summary>
        /// 队列的当前数据个数 
        /// </summary>
        public int Count
        {
            get { return _queue.Count; }

        }

        /// <summary>
        /// 移除并返回位于 Queue 开始处的对象。
        /// </summary>
        /// <returns>返回的条目</returns>
        public T Dequeue()
        {
            lock (this._queue)
            {
                if (this._queue.Count > 0)
                {
                    return this._queue.Dequeue();
                }
                else
                {
                    return default(T);
                }
            }
        }

        /// <summary>
        /// 将对象添加到 Queue的结尾处。
        /// </summary>
        /// <param name="item">加入的条目</param>
        public void Enqueue(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("Items null");
            }
            lock (this._queue)
            {
                this._queue.Enqueue(item);
            }
        }


        /// <summary>
        /// 如果队列达到了限定长度，就自动丢弃最前端的。
        /// 如果正常返回true,丢弃返回false.
        /// </summary>
        /// <param name="item">加入的条目</param>
        /// <returns>如果正常返回true,丢弃返回false</returns>
        public bool EnqueueMaxLimit(T item)
        {
            bool isDiscard = false;
            if (item == null)
            {
                throw new ArgumentNullException("DQueue.EnqueueMaxLimit():输入参数为null"); //注意其实下面的队列支持null
            }

            lock (this._queue)
            {
                if (_queue.Count < maxCount)
                {
                    _queue.Enqueue(item);
                }
                else
                {
                    _queue.Dequeue();
                    _queue.Enqueue(item);
                    isDiscard = true;
                }
            }
            return !isDiscard;
        }

        /// <summary>
        /// 如果队列达到了限定长度，就自动丢弃最前端的。
        /// 如果正常返回true,丢弃返回false.
        /// </summary>
        /// <param name="item">加入的条目</param>
        /// <param name="dequeueItem">最前端的</param>
        /// <returns>如果正常返回true,丢弃返回false</returns>
        public bool EnqueueMaxLimit(T item, out T dequeueItem)
        {
            bool isDiscard = false;
            dequeueItem = default(T);
            if (item == null)
            {
                throw new ArgumentNullException("DQueue.EnqueueMaxLimit():输入参数为null"); //注意其实下面的队列支持null
            }

            lock (this._queue)
            {
                if (_queue.Count < maxCount)
                {
                    _queue.Enqueue(item);
                }
                else
                {
                    dequeueItem = _queue.Dequeue();
                    _queue.Enqueue(item);
                    isDiscard = true;
                }
            }
            return !isDiscard;
        }

        /// <summary>
        /// 返回当前队列的整个数据拷贝,不会清空队列
        /// </summary>
        /// <returns>队列的数组拷贝</returns>
        public T[] ToArray()
        {
            lock (this._queue)
            {
                return _queue.ToArray();
            }
        }

        /// <summary>
        /// 清空队列
        /// </summary>
        public void Clear()
        {
            lock (this._queue)
            {
                this._queue.Clear();
            }
        }

        /// <summary>
        /// 如果元素数小于当前容量的 90%，将容量设置为队列中的实际元素数。
        /// </summary>
        public void TrimExcess()
        {
            lock (this._queue)
            {
                this._queue.TrimExcess();
            }
        }
    }


}
