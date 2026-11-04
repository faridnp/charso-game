using System;
using System.Collections.Generic;
using UnityEngine;

namespace ParsehNo.Contracts
{
    // Shared API boundary. Keep economy formulas, taxes and authoritative balances outside this assembly.
    public enum BuildableKind
    {
        Road, Residential, Commercial, Industrial, Recycling, Advanced, Warehouse,
        Park, Hospital, School, Power, Water, MilitaryBase, DefenseTower
    }

    [Serializable]
    public sealed class CityTileViewData
    {
        public int X;
        public int Z;
        public BuildableKind Kind;
        public int Level = 1;
    }

    [Serializable]
    public sealed class CityVisualSnapshot
    {
        public string CityName = "پارسه‌نو";
        public int GridSize = 36;
        public float TileSize = 2.2f;
        public long Revision;
        public List<CityTileViewData> Tiles = new List<CityTileViewData>();
    }

    public enum BuildFailureReason
    {
        None, OutOfBounds, Occupied, InsufficientFunds, Locked, InvalidKind, Unknown
    }

    public sealed class BuildCommandResult
    {
        public bool Success { get; private set; }
        public BuildFailureReason Failure { get; private set; }
        public string Message { get; private set; }
        public CityVisualSnapshot Snapshot { get; private set; }

        private BuildCommandResult(bool success, BuildFailureReason failure, string message, CityVisualSnapshot snapshot)
        {
            Success = success; Failure = failure; Message = message; Snapshot = snapshot;
        }

        public static BuildCommandResult Ok(CityVisualSnapshot snapshot, string message = "Placed")
            => new BuildCommandResult(true, BuildFailureReason.None, message, snapshot);
        public static BuildCommandResult Fail(BuildFailureReason reason, string message)
            => new BuildCommandResult(false, reason, message, null);
    }

    public interface ICityVisualReadModel
    {
        CityVisualSnapshot GetVisualSnapshot();
        event Action<CityVisualSnapshot> VisualStateChanged;
    }

    public interface ICityBuildCommandService
    {
        BuildCommandResult RequestPlacement(BuildableKind kind, int x, int z);
        BuildCommandResult RequestDemolition(int x, int z);
    }

    public interface ICityPresentationFacade : ICityVisualReadModel, ICityBuildCommandService { }
}
