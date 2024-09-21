﻿namespace ProductImporter.Utils
{
    public class IncrementingCounter : IIncrementingCounter
    {
        private int _counter = -1;

        public int GetNext() => ++_counter;
    }
}
