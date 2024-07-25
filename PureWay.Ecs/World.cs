namespace PureWay.Ecs;

public class World
{
	public int Create()
	{
		return 0;
	}

	public void Add<T>(int entity)
	{
	}

	public Filter<T> Filter<T>()
	{
		return new Filter<T>();
	}
}
