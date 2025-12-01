using UnityEngine;

namespace ContractsInterfaces.Infrastructure
{
    public interface IInputAdapter
    {
        Vector2 GetCameraMovement();
        float GetZoom();
        Vector2 GetMouseDrag();
        bool IsBuilding1Pressed();
        bool IsBuilding2Pressed();
        bool IsBuilding3Pressed();
        bool IsDeletePressed();
        bool IsRotatePressed();
    }
}

