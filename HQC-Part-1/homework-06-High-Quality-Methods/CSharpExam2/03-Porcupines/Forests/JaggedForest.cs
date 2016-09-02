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
                case MovementType.Jump:
                    newPosition = this.HandleMovement(newPosition, movement.Delta.Clone(), animal);
                    this.SetContentAtPosition(newPosition, ForestCellContentType.Rabbit);
                    break;
                case MovementType.Crawl:
                    newPosition = this.HandleMovement(newPosition, movement.Delta.Clone(), animal);
                    this.SetContentAtPosition(newPosition, ForestCellContentType.Porcupine);
                    break;
                default:
                    throw new ArgumentException("movement.MovementType");
            }

            this.SetContentAtPosition(startPosition, ForestCellContentType.Points);

            return newPosition;
        }

        private IPosition HandleMovement(IPosition startPosition, IPosition delta, IAnimal animal)
        {
            // Works - do not touch! 
            var collectedPoints = 0;

            var currentPosition = startPosition.Clone();
            var movesCount = this.GetMovesCount(delta);
            var crawlDelta = this.AdjustDeltaForCrawling(delta, movesCount);

            for (int move = 0; move < movesCount; move++)
            {
                var nextPosition = currentPosition.Add(crawlDelta);
                nextPosition = this.ValidateIndecesWithinForrestLimits(nextPosition, crawlDelta);

                if (animal.MovementType == MovementType.Crawl)
                {
                    var forestCellContent = this.forest[nextPosition.Row][nextPosition.Column].ContentType;
                    if (forestCellContent == ForestCellContentType.Rabbit)
                    {
                        nextPosition = nextPosition.Subtract(crawlDelta);
                        nextPosition = this.ValidateIndecesWithinForrestLimits(nextPosition, crawlDelta);
                        break;
                    }

                    collectedPoints += this.CollectPoints(nextPosition);
                }

                currentPosition = nextPosition.Clone();
            }

            if (animal.MovementType == MovementType.Jump)
            {
                var forestCellContent = this.forest[currentPosition.Row][currentPosition.Column].ContentType;
                if (forestCellContent == ForestCellContentType.Porcupine)
                {
                    currentPosition = currentPosition.Subtract(crawlDelta);
                    currentPosition = this.ValidateIndecesWithinForrestLimits(currentPosition, crawlDelta);
                }

                collectedPoints += this.CollectPoints(currentPosition);
            }

            animal.PointsCollected += collectedPoints;

            return currentPosition;
        }

        private IPosition ValidateIndecesWithinForrestLimits(IPosition nextPosition, IPosition delta)
        {
            IPosition validatedPosition;
            if (delta.Column != 0)
            {
                validatedPosition = this.ValidateNextHorizontalPositionWithinForestLimit(nextPosition.Clone());
            }
            else
            {
                validatedPosition = this.ValidateNextVerticalPositionWithinForestLimit(nextPosition.Clone());
            }

            return validatedPosition;
        }

        private IPosition ValidateNextVerticalPositionWithinForestLimit(IPosition positionToValidate)
        {
            var verticalSize = this.forest.Count - ((positionToValidate.Column / this.baseColumnsCount) * 2);

            var firstRowIndex = positionToValidate.Column / this.baseColumnsCount;
            while (positionToValidate.Row < firstRowIndex)
            {
                positionToValidate.Row += verticalSize;
            }

            while (this.forest.Count - firstRowIndex <= positionToValidate.Row)
            {
                positionToValidate.Row -= verticalSize;
            }

            return positionToValidate;
        }

        private IPosition ValidateNextHorizontalPositionWithinForestLimit(IPosition positionToValidate)
        {
            var horizontalSize = this.forest[positionToValidate.Row].Count;
            while (positionToValidate.Column < 0)
            {
                positionToValidate.Column += horizontalSize;
            }

            while (horizontalSize <= positionToValidate.Column)
            {
                positionToValidate.Column -= horizontalSize;
            }

            return positionToValidate;
        }

        private IPosition AdjustDeltaForCrawling(IPosition delta, int movesCount)
        {
            var adjustedDelta = delta.Clone();
            adjustedDelta.Row /= movesCount;
            adjustedDelta.Column /= movesCount;

            return adjustedDelta;
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
