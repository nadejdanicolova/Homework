﻿using System;
using System.Collections.Generic;

using _03_Porcupines.Animals.Contracts;
using _03_Porcupines.Animals.Enums;
using _03_Porcupines.Engine.Contracts;
using _03_Porcupines.Forests.Contracts;
using _03_Porcupines.Forests.Enums;

namespace _03_Porcupines.Forests
{
    public class JaggedForest : IForest
    {
        private IList<IList<IForestCell>> forest;
        private int baseColumnsCount;

        public JaggedForest(int rowsCount, int baseColumnsCount, IForestCellFactory forestCellFactory)
        {
            this.baseColumnsCount = baseColumnsCount;
            this.forest = this.BuildTheForest(rowsCount, baseColumnsCount);
            this.forest = this.FillTheForest(this.forest, ForestCellContentType.Points, forestCellFactory);
        }

        private int CollectPoints(IPosition position)
        {
            var collectedPoints = 0;

            var forestCell = this.forest[position.Row][position.Column];
            if (!forestCell.IsCollected)
            {
                forestCell.IsCollected = true;
                collectedPoints = forestCell.Points;
            }

            return collectedPoints;
        }

        public void SetContentAtPosition(IPosition position, ForestCellContentType contentType)
        {
            if (position == null)
            {
                throw new ArgumentException("position");
            }

            this.forest[position.Row][position.Column].ContentType = contentType;
        }

        public IPosition EvaluateMovement(IPosition startPosition, IMovement movement, IAnimal animal)
        {
            animal.PointsCollected += this.CollectPoints(startPosition);

            var newPosition = startPosition.Clone();
            switch (movement.MovementType)
            {
                case Animals.Enums.MovementType.Jump:
                    newPosition = this.HandleMovement(newPosition, movement.Delta.Clone(), animal);
                    this.SetContentAtPosition(newPosition, ForestCellContentType.Rabbit);
                    break;
                case Animals.Enums.MovementType.Crawl:
                    newPosition = this.HandleMovement(newPosition, movement.Delta.Clone(), animal);
                    this.SetContentAtPosition(newPosition, ForestCellContentType.Porcupine);
                    break;
                default:
                    throw new ArgumentException("movement.MovementType");
            }

            this.SetContentAtPosition(startPosition, ForestCellContentType.Points);

            return newPosition;
        }

        private IPosition HandleMovement(IPosition currentPosition, IPosition delta, IAnimal animal)
        {
            // TODO: FIX THIS CRAP
            var collectedPoints = 0;
            var movesCount = this.GetMovesCount(delta);
            delta = this.AdjustDeltaForCrawling(delta, movesCount);
            for (int move = 0; move < movesCount; move++)
            {
                var nextPosition = currentPosition.Add(delta);
                currentPosition = this.ValidateWithinLimits(nextPosition, delta);

                if (animal.MovementType == MovementType.Crawl)
                {
                    if (this.forest[currentPosition.Row][currentPosition.Column].ContentType == ForestCellContentType.Rabbit)
                    {
                        currentPosition = currentPosition.Subtract(delta);
                        currentPosition = this.ValidateWithinLimits(currentPosition, delta);
                        break;
                    }

                    collectedPoints += this.CollectPoints(currentPosition);
                }
            }

            if (animal.MovementType == MovementType.Jump)
            {
                currentPosition = this.ValidateCellDoesNotHaveAPorcupine(currentPosition, delta);
                collectedPoints += this.CollectPoints(currentPosition);
            }

            animal.PointsCollected += collectedPoints;

            return currentPosition;
        }

        private IPosition ValidateCellDoesNotHaveAPorcupine(IPosition currentPosition, IPosition delta)
        {
            var cellContent = this.forest[currentPosition.Row][currentPosition.Column].ContentType;
            if (cellContent == ForestCellContentType.Porcupine)
            {
                currentPosition = currentPosition.Subtract(delta);
                currentPosition = this.ValidateWithinLimits(currentPosition, delta);
            }

            return currentPosition;
        }

        private IPosition ValidateWithinLimits(IPosition nextPosition, IPosition delta)
        {
            IPosition currentPosition;
            if (delta.Column != 0)
            {
                currentPosition = this.ValidateNextHorizontalPositionWithinForestLimit(nextPosition.Clone());
            }
            else
            {
                currentPosition = this.ValidateNextVerticalPositionWithinForestLimit(nextPosition.Clone());
            }

            return currentPosition;
        }

        private IPosition ValidateNextVerticalPositionWithinForestLimit(IPosition nextPosition)
        {
            var verticalSize = this.forest.Count - ((nextPosition.Column / this.baseColumnsCount) * 2);

            var firstRowIndex = (nextPosition.Column / this.baseColumnsCount);
            while (nextPosition.Row < firstRowIndex)
            {
                nextPosition.Row += verticalSize;
            }

            while (this.forest.Count - firstRowIndex <= nextPosition.Row)
            {
                nextPosition.Row -= verticalSize;
            }

            return nextPosition;
        }

        private IPosition ValidateNextHorizontalPositionWithinForestLimit(IPosition nextPosition)
        {
            var horizontalSize = this.forest[nextPosition.Row].Count;
            while (nextPosition.Column < 0)
            {
                nextPosition.Column += horizontalSize;
            }

            while (horizontalSize <= nextPosition.Column)
            {
                nextPosition.Column -= horizontalSize;
            }

            return nextPosition;
        }

        private IPosition AdjustDeltaForCrawling(IPosition delta, int movesCount)
        {
            var newDelta = delta.Clone();
            newDelta.Row /= movesCount;
            newDelta.Column /= movesCount;

            return newDelta;
        }

        private int GetMovesCount(IPosition delta)
        {
            var movesCount = 0;
            if (delta.Row != 0)
            {
                movesCount = Math.Abs(delta.Row);
            }
            else
            {
                movesCount = Math.Abs(delta.Column);
            }

            return movesCount;
        }

        private IList<IList<IForestCell>> BuildTheForest(int rowsCount, int baseColumnsCount)
        {
            var forestColumnsMultiplier = 1;
            var forest = new IForestCell[rowsCount][];
            for (int row = 0; row < rowsCount / 2; row++)
            {
                var colsToAdd = forestColumnsMultiplier * baseColumnsCount;
                forestColumnsMultiplier++;

                forest[row] = new IForestCell[colsToAdd];
                forest[rowsCount - 1 - row] = new IForestCell[colsToAdd];
            }

            return forest;
        }

        private IList<IList<IForestCell>> FillTheForest(
            IList<IList<IForestCell>> forest,
            ForestCellContentType forestCellContent,
            IForestCellFactory forestCellFactory)
        {
            for (int row = 0; row < forest.Count; row++)
            {
                for (int column = 0; column < forest[row].Count; column++)
                {
                    var pointsValue = this.CalculatePointsValue(row, column);
                    var newForestCell = forestCellFactory.CreateForestCell(forestCellContent, pointsValue);
                    forest[row][column] = newForestCell;
                }
            }

            return forest;
        }

        private int CalculatePointsValue(int row, int column)
        {
            var rowValue = row + 1;
            var columnValue = column + 1;
            var cellValue = rowValue * columnValue;

            return cellValue;
        }
    }
}
