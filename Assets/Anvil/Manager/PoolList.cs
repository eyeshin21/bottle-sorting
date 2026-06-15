using System;

namespace Anvil
{
    public class LinkedNode<T> where T : new()
    {
        private T _value;
        private LinkedNode<T> _next;

        public T Value
        {
            get => _value;
            set => _value = value;
        }

        public LinkedNode<T> Next
        {
            get => _next;
            set => _next = value;
        }

        public LinkedNode()
        {
            _value = new T();
        }

        public LinkedNode(T value)
        {
            _value = value;
        }
    }

    class LinkedList<T> where T : new()
    {
        private LinkedNode<T> _head;
        private LinkedNode<T> _tail;

        public int Count
        {
            get
            {
                int count = 0;
                var node = _head;
                while (node != null)
                {
                    count++;
                    node = node.Next;
                }

                return count;
            }
        }

        public bool IsEmpty()
        {
            return _head == null;
        }

        public void Add(LinkedNode<T> node)
        {

            if (_head == null)
            {
                _head = node;
                _tail = node;
            }
            else
            {
                _tail.Next = node;
                _tail = node;
            }
        }

        /// <summary>
        /// Append the specified list to end of this and clear list.
        /// </summary>
        public void Append(LinkedList<T> list)
        {
            if (list._head != null)
            {
                if (_head != null)
                {
                    _tail.Next = list._head;
                }
                else
                {
                    _head = list._head;
                }

                _tail = list._tail;

                list._head = null;
                list._tail = null;
            }
        }

        public LinkedNode<T> RemoveFirst()
        {
            if (_head != null)
            {
                var node = _head;
                if (_head == _tail)
                {
                    _head = null;
                    _tail = null;
                }
                else
                {
                    _head = _head.Next;
                }
                node.Next = null;

                return node;
            }

            return null;
        }

        public void Browse(Action<T> callback)
        {
            var node = _head;
            while (node != null)
            {
                callback(node.Value);
                node = node.Next;
            }
        }

        /// <summary>
        /// func(node, remove)
        /// </summary>
        public void Browse(Func<T, bool> func, LinkedPool<T> pool)
        {
            if (_head == null) return;

            while (func(_head.Value))
            {
                var head = _head;
                _head = _head.Next;
                head.Next = null;
                pool.Return(head);

                if (_head == null)
                {
                    _tail = null;
                    return;
                }
            }

            var prevNode = _head;
            var node = prevNode.Next;

            while (node != null)
            {
                var nextNode = node.Next;
                if (func(node.Value))
                {
                    prevNode.Next = nextNode;

                    node.Next = null;
                    pool.Return(node);

                    if (node == _tail)
                    {
                        _tail = prevNode;
                        return;
                    }
                }
                else
                {
                    prevNode = node;
                }

                node = nextNode;
            }
        }

        public void ReturnToPool(LinkedPool<T> pool)
        {
            while (_head != null)
            {
                var head = _head;
                _head = _head.Next;

                head.Next = null;
                pool.Return(head);
            }

            _tail = null;
        }
    }

    class LinkedPool<T> : LinkedList<T> where T : new()
    {
        public Action<T> ReturnCallback { get; set; }

        public void Return(LinkedNode<T> node)
        {
            ReturnCallback?.Invoke(node.Value);
            Add(node);
        }
    }

    public class PoolList<T> where T : new()
    {
        private LinkedPool<T> _pool = new LinkedPool<T>();
        private LinkedList<T> _list = new LinkedList<T>();
        private LinkedList<T> _addList = new LinkedList<T>();

        public Action<T> ReturnCallback
        {
            get => _pool.ReturnCallback;
            set => _pool.ReturnCallback = value;
        }

        public int Count => _list.Count + _addList.Count;

        public bool IsEmpty()
        {
            return _list.IsEmpty() && _addList.IsEmpty();
        }

        public LinkedNode<T> Get()
        {
            var node = _pool.RemoveFirst();
            if (node == null)
            {
                node = new LinkedNode<T>();
            }

            return node;
        }

        public void Add(LinkedNode<T> node)
        {
            _addList.Add(node);
        }

        /// <summary>
        /// Browses adding list and current list.
        /// </summary>
        public void Browse(Action<T> callback)
        {
            _addList.Browse(callback);
            _list.Browse(callback);
        }

        /// <summary>
        /// Appends adding list to current list.
        /// func(node, remove)
        /// </summary>
        public void Browse(Func<T, bool> func)
        {
            _list.Append(_addList);
            _list.Browse(func, _pool);
        }

        public void Clear()
        {
            _list.ReturnToPool(_pool);
            _addList.ReturnToPool(_pool);
        }
    }
}
