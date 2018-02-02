﻿using System;
using System.Diagnostics;
using TShockAPI;
using Wolfje.Plugins.SEconomy;

namespace NpcShops.Shops
{
    /// <summary>
    ///     Represents a shop command.
    /// </summary>
    public sealed class ShopCommand
    {
        private readonly ShopCommandDefinition _definition;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ShopCommand" /> class with the specified definition.
        /// </summary>
        /// <param name="definition">The definition, which must not be <c>null</c>.</param>
        public ShopCommand(ShopCommandDefinition definition)
        {
            Debug.Assert(definition != null, "Definition must not be null.");

            _definition = definition;
            StackSize = definition.StackSize;
        }

        /// <summary>
        ///     Gets the command.
        /// </summary>
        public string Command => _definition.Command;

        /// <summary>
        ///     Gets the name.
        /// </summary>
        public string Name => _definition.Name;

        /// <summary>
        ///     Gets the permission required.
        /// </summary>
        //public string PermissionRequired => _definition.PermissionRequired;

        /// <summary>
        ///     Gets or sets the stack size. A value of -1 indicates unlimited.
        /// </summary>
        public int StackSize { get; set; }

        /// <summary>
        ///     Gets the unit price.
        /// </summary>
        public Money UnitPrice => _definition.UnitPrice;

        /// <summary>
        ///     Restocks the shop command.
        /// </summary>
        public void Restock()
        {
            StackSize = _definition.StackSize;
        }

		/// <summary>
		/// Allows a TSPlayer to run a command, regardless of permissions, while not being restricted to the Server player instance. 
		/// </summary>
		/// <param name="player"></param>
		/// <param name="commandString"></param>
		public void ForceHandleCommand(TSPlayer player)
		{
			//ripped from the sudo command at https://github.com/QuiCM/EssentialsPlus/blob/master/EssentialsPlus/Commands.cs
			var fakePlayer = new TSPlayer(player.Index)
			{
				AwaitingName = player.AwaitingName,
				AwaitingNameParameters = player.AwaitingNameParameters,
				AwaitingTempPoint = player.AwaitingTempPoint,
				Group = new SuperAdminGroup(),// : player.Group,
				TempPoints = player.TempPoints
			};

			//await Task.Run(() => TShockAPI.Commands.HandleCommand(fakePlayer, TShock.Config.CommandSpecifier + command));

			TShockAPI.Commands.HandleCommand(fakePlayer, Command.Replace("$name", player.GetEscapedName()));

			player.AwaitingName = fakePlayer.AwaitingName;
			player.AwaitingNameParameters = fakePlayer.AwaitingNameParameters;
			player.AwaitingTempPoint = fakePlayer.AwaitingTempPoint;
			player.TempPoints = fakePlayer.TempPoints;
		}
    }
}
