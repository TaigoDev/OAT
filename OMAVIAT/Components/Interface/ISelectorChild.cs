using Microsoft.AspNetCore.Components;
using OMAVIAT.Components.Reusable.Zone;

namespace OMAVIAT.Components.Interface;

public interface ISelectorChild<T> where T : ComponentBase
{
	[CascadingParameter] public ZoneListComponent? Parent { get; set; }
	public void OnClick();
	public void OnChangeSelect(bool enable);
}