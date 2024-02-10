using System;
using Unity.Collections;
using Unity.Netcode;

namespace Player
{
    public struct PlayerData : IEquatable<PlayerData>, INetworkSerializable
    {
        public ulong ClientId;
        public int ColorIndex;
        public FixedString64Bytes playerName;
        public FixedString64Bytes playerLobbyId;

        public bool Equals(PlayerData other)
        {
            return ClientId == other.ClientId && ColorIndex == other.ColorIndex && playerName == other.playerName
                   && playerLobbyId == other.playerLobbyId;
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref ClientId);
            serializer.SerializeValue(ref ColorIndex);
            serializer.SerializeValue(ref playerName);
            serializer.SerializeValue(ref playerLobbyId);
        }
    }
}