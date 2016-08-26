﻿using System;

using Minesweeper.Contracts;

namespace Minesweeper.Engine
{
    /// <summary>
    /// Implaments IEngine for Minesweeper.
    /// </summary>
    public class MinesweeperEngine : IGameEngine
    {
        private const string PlayEmptyMessageTemplate = "Success! Position row: {0}, col: {1} is empty!";
        private const string PlayMineMessageTemplate = "BOOM! There is a mine at row: {0}, col: {1}! Play again?";
        private const string EndGameMessage = "Seeya later aligator!";

        private const string TopScoreCommand = "top";
        private const string RestartCommand = "restart";
        private const string ClickCommand = "click";
        private const string ExitCommand = "exit";

        private const int WinConditionSuccsessfulTurns = 35;

        private IGameBoard gameBoard;
        private IScoreBoard scoreBoard;
        private IUserInterface userInterface;

        private bool isRunning;

        /// <summary>
        /// Create a new GameCommandsEngine.
        /// </summary>
        /// <param name="gameBoard"> Required to check against. </param>
        /// <param name="scoreBoard"> Required to keep track of score history. </param>
        /// <param name="userInterface"> Required to display the game to the user. </param>
        public MinesweeperEngine(IGameBoard gameBoard, IScoreBoard scoreBoard, IUserInterface userInterface)
        {
            this.CheckIfConstructorObjectIsNull(gameBoard);
            this.CheckIfConstructorObjectIsNull(scoreBoard);
            this.CheckIfConstructorObjectIsNull(userInterface);

            this.gameBoard = gameBoard;
            this.scoreBoard = scoreBoard;
            this.userInterface = userInterface;
        }

        /// <summary>
        /// Returns whether the app should keep feeding commands to the engine.
        /// </summary>
        public bool IsRunning
        {
            get
            {
                return this.isRunning;
            }
        }

        /// <summary>
        /// Parse and execute a string command.
        /// Commands : 'top', 'restart', 'exit', '{row} {col}'
        /// </summary>
        /// <param name="command"> String to be parsed. </param>
        /// <returns> Returns False when command is 'Exit'. </returns>
        public void ExecuteNextCommand(string command)
        {
            var continueGameExecution = true;

            var commandWords = this.ConvertCommandString(command);
            switch (commandWords[0])
            {
                case MinesweeperEngine.TopScoreCommand:
                    this.HandleTopScoreCommand();
                    break;
                case MinesweeperEngine.RestartCommand:
                    this.HandleRestartCommand();
                    break;
                case MinesweeperEngine.ClickCommand:
                    // TODO:
                    break;
                case MinesweeperEngine.ExitCommand:
                    continueGameExecution = false;
                    break;
                default:
                    // TODO: 
                    break;
            }

            this.isRunning = continueGameExecution;
        }

        private void HandleTopScoreCommand()
        {
            var scoreListToDisplay = this.scoreBoard.GetTopScores(5);
            this.userInterface.DisplayHighScore(scoreListToDisplay);
        }

        private void HandleRestartCommand()
        {
            this.gameBoard.ResetGameBoard();
        }

        private void AddNewScoreCardToScoreBoard(string name, int score)
        {

        }

        private string[] ConvertCommandString(string command)
        {
            var convertedCommand = command.Split(' ');

            return convertedCommand;
        }

        private void CheckIfConstructorObjectIsNull(object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(obj.GetType().ToString());
            }
        }
    }
}
