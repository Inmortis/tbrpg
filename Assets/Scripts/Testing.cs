using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    private Grid<HeatMapGridObject> heatMapGrid; // Сетка для тепловой карты

    private void Start()
    {
        // Создаем сетку для тепловой карты
        heatMapGrid = new Grid<HeatMapGridObject>(
            20, 10, 8f, Vector3.zero,
            (Grid<HeatMapGridObject> g, int x, int y) => new HeatMapGridObject(g, x, y)
        );
    }

    private void Update()
    {
        Vector3 position = Exlib.GetMouseWorldPosition();

        // Добавляем тепловое значение по клику левой кнопки мыши
        if (Input.GetMouseButtonDown(0))
        {
            HeatMapGridObject heatMapGridObject = heatMapGrid.GetGridObject(position);
            if (heatMapGridObject != null)
            {
                heatMapGridObject.AddValue(5);
                Debug.Log($"Heat value at {position}: {heatMapGridObject}");
            }
        }

        // Уменьшаем тепловое значение по клику правой кнопки мыши
        if (Input.GetMouseButtonDown(1))
        {
            HeatMapGridObject heatMapGridObject = heatMapGrid.GetGridObject(position);
            if (heatMapGridObject != null)
            {
                heatMapGridObject.AddValue(-5);
                Debug.Log($"Heat value at {position}: {heatMapGridObject}");
            }
        }
    }
}

public class HeatMapGridObject
{
    private const int MIN = 0; // Минимальное значение
    private const int MAX = 100; // Максимальное значение

    private Grid<HeatMapGridObject> grid;
    private int x;
    private int y;
    private int value; // Значение тепловой карты

    public HeatMapGridObject(Grid<HeatMapGridObject> grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
        this.value = 0; // Инициализация начального значения
    }

    public void AddValue(int addValue)
    {
        value += addValue;
        value = Mathf.Clamp(value, MIN, MAX); // Ограничиваем значение в пределах MIN и MAX
        grid.TriggerGridObjectChanged(x, y);
    }

    public override string ToString()
    {
        return value.ToString();
    }
}
