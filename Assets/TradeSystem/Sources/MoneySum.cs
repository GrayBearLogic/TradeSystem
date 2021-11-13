using System;
using UnityEngine;
using UnityEngine.Events;

namespace TradeSystem
{
    /// <summary>
    /// Some sum of money.
    /// </summary>
    public class MoneySum
    {
        /// <summary>
        /// Occurs when the amount of money increases.
        /// </summary>
        public UnityEvent<MoneySum, int> moneyGot;
        /// <summary>
        /// Occurs when the amount of money decreases.
        /// </summary>
        public UnityEvent<MoneySum, int> moneyLost;
        /// <summary>
        /// Occurs when the amount of money changes.
        /// </summary>
        public UnityEvent<MoneySum, int> amountChanged;

        /// <summary>
        /// Create the new sum of money.
        /// </summary>
        /// <param name="id">Is as a key for storing the value.</param>
        /// <param name="loadOnStart">Load initial value from the disk</param>
        /// <param name="startAmount">The initial amount of money.</param>
        public MoneySum(string id, bool loadOnStart = false, int startAmount = 0)
        {
            Id = id;
            Amount = loadOnStart ? PlayerPrefs.GetInt(Id, startAmount) : startAmount;
            moneyGot = new UnityEvent<MoneySum, int>();
            moneyLost = new UnityEvent<MoneySum, int>();
            amountChanged = new UnityEvent<MoneySum, int>();
        }

        /// <summary>
        /// The amount of money in this sum.
        /// </summary>
        public int Amount
        {
            get;
            private set;
        }
        /// <summary>
        /// Is used as a key for saving values.
        /// </summary>
        public string Id
        {
            get;
        }

        /// <summary>
        /// Does the amount of money in this sum is greater or equal to <code>amount</code>
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public bool HasAtLeast(int amount)
        {
            return Amount >= amount;
        }
        /// <summary>
        /// Add some money to this sum.
        /// </summary>
        /// <param name="amount"></param>
        public void Add(int amount)
        {
            Amount += amount;
            moneyGot.Invoke(this, amount);
            amountChanged.Invoke(this, Amount);
        }
        private void Remove(int amount)
        {
            Amount -= amount;
            moneyLost.Invoke(this, amount);
            amountChanged.Invoke(this, Amount);
        }

        /// <summary>
        /// Multiply the <code>Amount</code> by some factor.
        /// </summary>
        /// <param name="factor"></param>
        public void Multiply(float factor)
        {
            var oldAmount = Amount;
            Amount = Convert.ToInt32(oldAmount * factor);
            var difference = Amount - oldAmount;
            
            if (difference > 0)
            {
                moneyGot?.Invoke(this, difference);
                amountChanged?.Invoke(this, Amount);
            }
            else if (difference < 0)
            {
                moneyLost?.Invoke(this, difference);
                amountChanged?.Invoke(this, Amount);
            }
        }
        /// <summary>
        /// Remove all money from another sum and add them to this sum.
        /// </summary>
        /// <param name="another"></param>
        public void AddFrom(MoneySum another)
        {
            this.Add(another.Amount);
            another.Remove(another.Amount);
        }
        /// <summary>
        /// Remove some amount of money from another sum and add them to this sum
        /// if another sum has enough money inside.
        /// </summary>
        /// <param name="another"></param>
        /// <param name="amount">The amount of money to be removed</param>
        /// <returns>Whether the method successfully executed.</returns>
        public bool TryAddFrom(MoneySum another, int amount)
        {
            if (another.HasAtLeast(amount))
            {
                this.Add(amount);
                another.Remove(amount);
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Remove some amount of money from the sum if it has enough money inside.
        /// </summary>
        /// <param name="amount"></param>
        /// <returns>Whether the method successfully executed.</returns>
        public bool TryRemove(int amount)
        {
            if (HasAtLeast(amount))
            {
                Remove(amount);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Save the amount of money to the disk.
        /// </summary>
        public void Save()
        {
            PlayerPrefs.SetInt(Id, Amount);
            PlayerPrefs.Save();
        }
        /// <summary>
        /// Load the amount of money from the disk.
        /// </summary>
        public void Load()
        {
            Amount = PlayerPrefs.GetInt(Id);
        }
        
    }
}
