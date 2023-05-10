using System;
using System.Collections.Generic; // Need to add this namespace for List<T>

namespace DesignPatterns.ObserverState
{
    public interface IObserver
    {
        void Update(State state); // Update method should take a State object, not ISubject
    }

    public interface ISubject
    {
        void Attach(IObserver observer);

        void Detach(IObserver observer);

        void Notify(State state);
    }

    public class Subject : ISubject
    {
        private List<IObserver> _observers = new List<IObserver>();

        public void Attach(IObserver observer)
        {
            Console.WriteLine("Subject: Attached an observer.");
            this._observers.Add(observer);
        }

        public void Detach(IObserver observer)
        {
            this._observers.Remove(observer);
            Console.WriteLine("Subject: Detached an observer.");
        }

        public void Notify(State state)
        {
            Console.WriteLine("Subject: Notifying observers...");

            foreach (var observer in _observers)
            {
                observer.Update(state);
            }
        }
    }

    class LoginObserver : IObserver
    {
        public void Update(State state)
        {
            if (state.GetType() == typeof(AuthenticatedState)) // Fixed typo in class name
            {
                Console.WriteLine("LoginObserver: Reacted to the event.");
            }
        }
    }

    class LogoutObserver : IObserver
    {
        public void Update(State state)
        {
            if (state.GetType() == typeof(UnauthenticatedState)) // Fixed typo in class name
            {
                Console.WriteLine("LogoutObserver: Reacted to the event.");
            }
        }
    }

    public class Context
    {
        // A reference to the current state of the Context.
        private State _state = null;
        public Subject subject = new Subject();

        public Context(State state)
        {
            this.TransitionTo(state);
        }

        public void TransitionTo(State state)
        {
            Console.WriteLine($"Context: Transition to {state.GetType().Name}.");
            this._state = state;
            this._state.SetContext(this);
        }

        public void LoginRequest()
        {
            this._state.LoginHandle(); // Fixed typo in method name
        }

        public void LogoutRequest()
        {
            this._state.LogoutHandle();
        }

        public void PayRequest()
        {
            this._state.PayHandle();
        }
    }

    public abstract class State
    {
        protected Context _context;

        public void SetContext(Context context)
        {
            this._context = context;
        }

        public abstract void LoginHandle(); // Fixed typo in method name

        public abstract void LogoutHandle();

        public abstract void PayHandle();
    }

    class UnauthenticatedState : State // Fixed typo in class name
    {
        public override void LoginHandle()
        {
            Console.WriteLine("Login Success"); // Fixed typo in message
            this._context.TransitionTo(new AuthenticatedState()); // Fixed typo in class name
            this._context.subject.Notify(this); // Notify observers of state change
        }

        public override void LogoutHandle()
        {
            Console.WriteLine("Already unauthenticated"); // Fixed typo in message
        }

        public override void PayHandle()
        {
            Console.WriteLine("Can't pay, unauthenticated"); // Fixed typo in message
        }
    }

    class AuthenticatedState : State
    {
        public override void LoginHandle()
        {
            Console.WriteLine("Already authenticated"); // Fixed typo in message
        }

        public override void LogoutHandle()
        {
            Console.WriteLine("Logout Success"); // Fixed typo in message
            this._context.TransitionTo(new UnauthenticatedState());
            this._context.subject.Notify(this); // Notify observers of state change
    }

    public override void PayHandle()
    {
        Console.WriteLine("Payment Successful"); // Fixed typo in message
    }
}

class Program
{
    static void Main(string[] args)
    {
        var context = new Context(new UnauthenticatedState());

        var loginObserver = new LoginObserver();
        context.subject.Attach(loginObserver);

        var logoutObserver = new LogoutObserver();
        context.subject.Attach(logoutObserver);

        context.LoginRequest();
        context.PayRequest();
        context.LogoutRequest();
    }
}
}
