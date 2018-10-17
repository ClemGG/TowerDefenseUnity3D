using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generics : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Dictionary<string, int> dico = new Dictionary<string, int>(1);
        dico.Add("Kamasutra", 100);
        print(dico["Kamasutra"]);

        print(Utility.CompareValues("0", 0));
        print(Utility.CompareTypes(1, 0));
	}
	
}

public class Dictionnaire<Tkey, TValue> // Les noms TKey et TValue sont optionnels, on peut mettre d'autres noms si on veut
{
    public Tkey key;
    public TValue val;

    public Dictionnaire(Tkey key, TValue val)
    {
        this.key = key;
        this.val = val;
    }
}


public class Utility
{
    public static bool CompareValues<Type1, Type2> (Type1 value1, Type2 value2)
    {
        return value1.Equals(value2);
    }
    public static bool CompareTypes<Type1, Type2> (Type1 value1, Type2 value2)
    {
        //return value1.GetType().Equals(value2.GetType());
        return typeof(Type1).Equals(typeof(Type2));
    }
}
