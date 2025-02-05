using Microsoft.AspNetCore.Components;

namespace OMAVIAT.Components.Interface;

public interface ISelectorParent<T> where T : ComponentBase
{
	public List<T> Childs { get; set; }
	public void Select(T child);
}