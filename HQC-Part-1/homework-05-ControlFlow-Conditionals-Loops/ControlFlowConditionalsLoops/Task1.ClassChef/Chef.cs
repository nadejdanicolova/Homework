﻿using Task1.ClassChef.Contracts;

namespace Task1.ClassChef
{
    public class Chef : IChef
    {
        public IMeal Cook(IOven oven)
        {
            var potato = this.GetPotato();
            var carrot = this.GetCarrot();

            var peeledPotato = this.Peel(potato);
            var peeledCarrot = this.Peel(carrot);

            var cutPotato = this.Cut(peeledPotato);
            var curCarrot = this.Cut(peeledCarrot);

            var bowl = this.GetBowl();
            var bowlWithVegetables = bowl.Add(curCarrot).Add(cutPotato);

            var preparedMeal = oven.Cook(bowlWithVegetables);
            return preparedMeal;
        }

        private IVegetable Cut(IVegetable vegetable)
        {
            vegetable.IsCut = true;

            return vegetable;
        }

        private IVegetable Peel(IVegetable vegetable)
        {
            vegetable.IsPealed = true;

            return vegetable;
        }

        private IPotato GetPotato()
        {
            IPotato potato = null;

            return potato;
        }

        private ICarrot GetCarrot()
        {
            ICarrot carrot = null;

            return carrot;
        }

        private IBowl GetBowl()
        {
            IBowl bowl = null;

            return bowl;
        }
    }
}
