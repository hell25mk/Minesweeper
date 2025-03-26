using UnityEngine;
using System;

public enum BuriedItemType {
    Empty = 0,
    One,
    Two,
    Three,
    Four,
    Five,
    Six,
    Seven,
    Eight,
    Mine
}

public static class BuriedItemTypeExtension {
    private static BuriedItemType[] itemTypes = (BuriedItemType[])Enum.GetValues(typeof(BuriedItemType));

    public static BuriedItemType Next(this BuriedItemType value) {
        int next = Array.IndexOf(itemTypes, value) + 1;

        if(next >= itemTypes.Length) {
            return value;
        }

        return itemTypes[next];
    }

    public static BuriedItemType Prev(this BuriedItemType value) {
        int prev = Array.IndexOf(itemTypes, value) - 1;

        if(prev < 0) {
            return value;
        }

        return itemTypes[prev];
    }
}
