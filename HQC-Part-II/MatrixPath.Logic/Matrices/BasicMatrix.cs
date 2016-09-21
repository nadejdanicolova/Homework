﻿using System;
using System.Collections.Generic;

using MatrixPath.Logic.Cells.Contracts;
using MatrixPath.Logic.Directions.Contracts;
using MatrixPath.Logic.Matrices.Contracts;
using MatrixPath.Logic.Values.Contracts;

namespace MatrixPath.Logic.Matrices
{
    public class BasicMatrix : IMatrix
    {
        private IList<IList<ICell>> theMatrix;

        public BasicMatrix(int matrixSize, Func<int, int, ICell> createCell)
        {
            if (matrixSize <= 0)
            {
                throw new ArgumentOutOfRangeException("Matrix size must be larger than 0");
            }

            if (createCell == null)
            {
                throw new ArgumentNullException("createCell");
            }

            this.theMatrix = this.CreateTheMatrix(matrixSize, createCell);
        }

        public string Populate(IDirectionSequence directionsInstructions, ICellValueSequence values)
        {
            throw new NotImplementedException();
        }

        private IList<IList<ICell>> CreateTheMatrix(int size, Func<int, int, ICell> createCell)
        {
            var theMatrix = new List<IList<ICell>>();
            for (int rowIndex = 0; rowIndex < size; rowIndex++)
            {
                theMatrix[rowIndex] = new List<ICell>();
                for (int colIndex = 0; colIndex < size; colIndex++)
                {
                    var cellToAdd = createCell(rowIndex, colIndex);
                    theMatrix[rowIndex].Add(cellToAdd);
                }
            }

            return theMatrix;
        }
    }
}
