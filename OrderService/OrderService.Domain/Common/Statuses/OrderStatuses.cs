using System.ComponentModel;

namespace OrderService.Domain.Common.Statuses;

public enum OrderStatuses
{
	[Description("Created")]
	Created,
	[Description("Completed")]
	Completed,
	[Description("Cancelled")]
	Cancelled
}