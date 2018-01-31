using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VW3D.Graphic
{
    class IndicesCollection
    {
        public List<int> Indices;

        public List<IndicesCollectionPointer> Pointers;

        IndicesCollectionPointer _currentPointer;

        public void Add(int meshSize, int count)
        {
            if (Indices == null)
            {
                Indices = new List<int>();
                _currentPointer = new IndicesCollectionPointer();
                Pointers = new List<IndicesCollectionPointer>();
            }

            if (_currentPointer.Stride != meshSize)
            {
                _currentPointer = new IndicesCollectionPointer(
                _currentPointer.Position + _currentPointer.Length,
                count,
                meshSize
                );

                Indices.AddRange(Enumerable.Range(_currentPointer.Position, _currentPointer.Length));
                Pointers.Add(_currentPointer);
            }
            else
            {
                _currentPointer = new IndicesCollectionPointer(
                _currentPointer.Position,
                _currentPointer.Length + count,
                meshSize
                );

                Indices.AddRange(Enumerable.Range((_currentPointer.Position + _currentPointer.Length - count), count));
                Pointers.RemoveAt(Pointers.Count - 1);
                Pointers.Add(_currentPointer);
            }
        }
    }
}
