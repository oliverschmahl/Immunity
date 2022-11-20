using Managers;
using UnityEngine;

namespace Helpers
{
    public static class WorldBounds
    {
        public static bool InsideWorld(Vector2 position)
        {
            Vector3[] worldCorners = GameManager.instance.GetWorldCorners();
            Vector3 bottomLeftCorner = worldCorners[0];
            Vector3 topLeftCorner = worldCorners[1];
            Vector3 topRightCorner = worldCorners[2];
            bool outsideRightBorder = position.x > topRightCorner.x - GameManager.instance.playableAreaPadding;
            bool outsideLeftBorder = position.x < topLeftCorner.x + GameManager.instance.playableAreaPadding;
            bool outsideBottomBorder = position.y < bottomLeftCorner.y - GameManager.instance.playableAreaPadding;
            bool outsideTopBorder = position.y > topRightCorner.y + GameManager.instance.playableAreaPadding;

            bool inside = !outsideRightBorder && !outsideLeftBorder && !outsideBottomBorder && !outsideTopBorder;

            return inside;
        }

        public static float GetMaxX()
        {
            Vector3[] worldCorners = GameManager.instance.GetWorldCorners();
            return worldCorners[2].x;
        }
        public static float GetMinX()
        {
            Vector3[] worldCorners = GameManager.instance.GetWorldCorners();
            return worldCorners[1].x;
        }
        public static float GetMaxY()
        {
            Vector3[] worldCorners = GameManager.instance.GetWorldCorners();
            return worldCorners[1].y;
        }
        public static float GetMinY()
        {
            Vector3[] worldCorners = GameManager.instance.GetWorldCorners();
            return worldCorners[0].y;
        }
    }
}