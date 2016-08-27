﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Minesweeper.Contracts;

namespace Minesweeper.Models
{
    /// <summary>
    /// Base implementation of IScoreBoard.
    /// </summary>
    public class ScoreBoard : IScoreBoard
    {
        private ConstructorInfo constructorToUse;

        private ICollection<IScoreCard> scores;

        /// <summary>
        /// Creates a new ScoreBoard storing data in the format provided with Type parameter.
        /// </summary>
        /// <param name="typeOfScoreCardToUse"> Type of IScoreCard to use. </param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public ScoreBoard(Type typeOfScoreCardToUse)
        {
            if (typeOfScoreCardToUse == null)
            {
                throw new ArgumentNullException("typeOfScoreCardToUse");
            }

            var getIScoreCardInterface = typeOfScoreCardToUse.GetInterface("IScoreCard");
            if (getIScoreCardInterface == null)
            {
                throw new ArgumentException("Incorrect type.");
            }

            var getConstructor = this.GetConstructorToUse(typeOfScoreCardToUse);
            if (getConstructor == null)
            {
                throw new ArgumentException("Constructor not found.");
            }

            this.constructorToUse = getConstructor;
            this.scores = new HashSet<IScoreCard>();
        }

        /// <summary>
        /// Add a new element to the IScoreBoard.
        /// </summary>
        /// <param name="name"> Name to associate the score with. </param>
        /// <param name="score"> Number of points. </param>
        public void AddScoreCard(string name, int score)
        {
            var newScoreCard = (IScoreCard)this.constructorToUse.Invoke(new object[] { name, score });
            this.scores.Add(newScoreCard);
        }

        /// <summary>
        /// Get the top number scores stored.
        /// </summary>
        /// <param name="number"> The number of scores to return. </param>
        /// <returns> IEnumerable containg the number of scores requested. </returns>
        public IList<IScoreCard> GetTopScores(int number)
        {
            var topScoresToReturn = this.scores
                .OrderBy(score => score.Score)
                .ThenBy(score => score.Name)
                .Take(number)
                .ToList();

            return topScoresToReturn;
        }

        private ConstructorInfo GetConstructorToUse(Type type)
        {
            var constructorToUse = type.GetConstructor(
                BindingFlags.Public | BindingFlags.Instance,
                null,
                new[] { typeof(string), typeof(int) },
                null);

            return constructorToUse;
        }
    }
}
