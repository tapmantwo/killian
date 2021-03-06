﻿using System;
using System.Collections.Generic;

namespace Runners
{
    public class Scenario<T>
    {
        public static Scenario<T> With(T subject)
        {
            return new Scenario<T>(subject);
        }

        private readonly T _subject;

        private readonly Queue<Action<T>> _initialisations;

        private readonly Queue<Action<T>> _operations;

        private readonly Queue<Action<T>> _assertions;

        private Scenario(T subject)
        {
            _subject = subject;
            _initialisations = new Queue<Action<T>>();
            _operations = new Queue<Action<T>>();
            _assertions = new Queue<Action<T>>();
        }

        public Scenario<T> Given(Action<T> initialisation)
        {
            _initialisations.Enqueue(initialisation);
            return this;
        }

        public Scenario<T> AndGiven(Action<T> initialisation)
        {
            return Given(initialisation);
        }

        public Scenario<T> HasHappened(Times times)
        {
            var lastInitialisation = _initialisations.Dequeue();
            for (var i = 0; i < times; i++)
            {
                _initialisations.Enqueue(lastInitialisation);
            }

            return this;
        }

        public Scenario<T> When(Action<T> action)
        {
            _operations.Enqueue(action);
            return this;
        }

        public Scenario<T> AndWhen(Action<T> action)
        {
            return When(action);
        }

        public Scenario<T> Happens(Times times)
        {
            var lastOperation = _operations.Dequeue();
            for (var i = 0; i < times; i++)
            {
                _operations.Enqueue(lastOperation);
            }

            return this;
        }

        public Scenario<T> Then(Action<T> assertion)
        {
            _assertions.Enqueue(assertion);
            return this;
        }

        public Scenario<T> AndThen(Action<T> assertion)
        {
            return Then(assertion);
        }

        public void Run()
        {
            while (_initialisations.Count > 0)
            {
                _initialisations.Dequeue().Invoke(_subject);
            }

            while (_operations.Count > 0)
            {
                _operations.Dequeue().Invoke(_subject);
            }

            while (_assertions.Count > 0)
            {
                _assertions.Dequeue().Invoke(_subject);
            }
        }
    }

    public class ScenarioWithResult<T,R>
    {
        public static ScenarioWithResult<T,R> With(T subject)
        {
            return new ScenarioWithResult<T,R>(subject);
        }

        private readonly T _subject;

        private R _result;

        private readonly Queue<Action<T>> _initialisations;

        private Func<T,R> _operation;

        private Times _operationTimes;

        private readonly Queue<Action<R>> _assertions;

        private ScenarioWithResult(T subject)
        {
            _subject = subject;
            _initialisations = new Queue<Action<T>>();
            _assertions = new Queue<Action<R>>();
            _operationTimes = Times.Once();
        }

        public ScenarioWithResult<T, R> Given(Action<T> initialisation)
        {
            _initialisations.Enqueue(initialisation);
            return this;
        }

        public ScenarioWithResult<T, R> AndGiven(Action<T> initialisation)
        {
            return Given(initialisation);
        }

        public ScenarioWithResult<T, R> HasHappened(Times times)
        {
            var lastInitialisation = _initialisations.Dequeue();
            for (var i = 0; i < times; i++)
            {
                _initialisations.Enqueue(lastInitialisation);
            }

            return this;
        }

        public ScenarioWithResult<T, R> When(Func<T,R> operation)
        {
            _operation = operation;
            return this;
        }

        public ScenarioWithResult<T, R> Happens(Times times)
        {
            _operationTimes = times;
            return this;
        }

        public ScenarioWithResult<T, R> Then(Action<R> assertion)
        {
            _assertions.Enqueue(assertion);
            return this;
        }

        public ScenarioWithResult<T, R> AndThen(Action<R> assertion)
        {
            return Then(assertion);
        }

        public void Run()
        {
            while (_initialisations.Count > 0)
            {
                _initialisations.Dequeue().Invoke(_subject);
            }

            for (var operationIndex = 0; operationIndex < _operationTimes; operationIndex++ )
            {
                _result = _operation.Invoke(_subject);
            }

            while (_assertions.Count > 0)
            {
                _assertions.Dequeue().Invoke(_result);
            }
        }
    }

    public class Times
    {
        private readonly int _howMany;

        protected Times(int howMany)
        {
            _howMany = howMany;
        }

        public static Times Once()
        {
            return new Times(1);
        }

        public static Times Many(int howMany)
        {
            return new Times(howMany);
        }

        public static implicit operator int(Times t)
        {
            return t._howMany;
        }
    }
}
