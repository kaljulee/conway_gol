using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Templates
{

    // toad
    //SpawnSites.AddFirst(new Vector2(2, 3));
    //SpawnSites.AddFirst(new Vector2(3, 3));
    //SpawnSites.AddFirst(new Vector2(4, 3));

 
    public static LinkedList<Vector2> GetTemplate(int templateType) {
        switch(templateType) {
            case TEMPLATE_TYPES.BLINKER:
                return Blinker();
            case TEMPLATE_TYPES.GLIDER:
                return Glider();
            case TEMPLATE_TYPES.SQUARE:
                return Square();
            default:
                return Square();
        }
    }

    public static class TEMPLATE_TYPES {
        public const int BLINKER = 0;
            public const int GLIDER = 1;
        public const int SQUARE = 2;
    }


    public static LinkedList<Vector2> Square() {
        LinkedList<Vector2> sites = new LinkedList<Vector2>();
        sites.AddFirst(new Vector2(0, 0));
        sites.AddFirst(new Vector2(1, 0));
        sites.AddFirst(new Vector2(0, 1));
        sites.AddFirst(new Vector2(1, 1));
        return sites;
    }

    public static LinkedList<Vector2> Blinker() {
        LinkedList<Vector2> sites = new LinkedList<Vector2>();
        sites.AddFirst(new Vector2(0, 0));
        sites.AddFirst(new Vector2(1, 0));
        sites.AddFirst(new Vector2(2, 0));
        return sites;
    }

    // reverse glider
    public static LinkedList<Vector2> Glider() {
        LinkedList<Vector2> sites = new LinkedList<Vector2>();
        sites.AddFirst(new Vector2(0, 1));
        sites.AddFirst(new Vector2(1, 1));
        sites.AddFirst(new Vector2(1, 0));
        sites.AddFirst(new Vector2(0, 2));
        sites.AddFirst(new Vector2(2, 2));
        return sites;
    }

    public static int FlipValue(int value) => value + (value * -2);
    
    public static LinkedList<Vector2> HorizontalFlip(LinkedList<Vector2> template) {
        LinkedList<Vector2> returnList = new LinkedList<Vector2>();
        foreach(Vector2 site in template) {
            returnList.AddLast(new Vector2(FlipValue((int)site.x), site.y));
        }
        return returnList;
    }

    public static LinkedList<Vector2> VerticalFlip(LinkedList<Vector2> template) {
        LinkedList<Vector2> returnList = new LinkedList<Vector2>();
        foreach (Vector2 site in template) {
            returnList.AddLast(
                new Vector2(
                    site.y, 
                    FlipValue(
                        (int)site.x)
                    )
                );
        }
        return returnList;
    }

}
