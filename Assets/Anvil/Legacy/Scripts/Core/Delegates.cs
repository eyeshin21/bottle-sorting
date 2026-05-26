using UnityEngine;

namespace Anvil.Legacy
{
    public delegate void Callback();
    public delegate void Callback<T>(T arg);
    public delegate void Callback<T1, T2>(T1 arg1, T2 arg2);
    public delegate void Callback<T1, T2, T3>(T1 arg1, T2 arg2, T3 arg3);

    public delegate void Listener();
    public delegate void Listener<T>(T arg);
    public delegate void Listener<T1, T2>(T1 arg1, T2 arg2);
    public delegate void Listener<T1, T2, T3>(T1 arg1, T2 arg2, T3 arg3);

    public delegate T AddCallback<T>();

    //public delegate void GetCallback<T>(T arg);
    //public delegate void GetCallback<T1, T2>(T1 arg1, T2 arg2);
    //public delegate void GetCallback<T1, T2, T3>(T1 arg1, T2 arg2, T2 arg3);

    //public delegate void SetCallback<T>(T arg);
    //public delegate void SetCallback<T1, T2>(T1 arg1, T2 arg2);
    //public delegate void SetCallback<T1, T2, T3>(T1 arg1, T2 arg2, T2 arg3);

    public delegate TResult Func<TResult>();
    public delegate TResult Func<T, TResult>(T arg);
    public delegate TResult Func<T1, T2, TResult>(T1 arg1, T2 arg2);

    public delegate bool AcceptFunc<T>(T arg);
    public delegate bool AcceptFunc<T1, T2>(T1 arg1, T2 arg2);
    public delegate bool ContinueFunc<T>(T arg);
    public delegate TResult ContinueFunc<T, TResult>(T arg);
    public delegate bool ChangedFunc<T>(T arg);

    public delegate bool Accepter<T>(T arg);

    public delegate TResult Getter<TResult>();
    public delegate TResult Getter<T, TResult>(T arg);

    public delegate void Setter<T>(T arg);
    public delegate void Setter<T1, T2>(T1 arg1, T2 arg2);

    public delegate void Handler<T>(T arg);
    public delegate bool HandlerFunc<T>(T arg);
}