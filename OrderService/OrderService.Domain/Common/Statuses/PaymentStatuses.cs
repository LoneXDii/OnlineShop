using System.ComponentModel;

namespace OrderService.Domain.Common.Statuses;

public enum PaymentStatuses 
{
	[Description("Paid")]
	Paid,
	[Description("Not paid")]
	NotPaid,
	[Description("Upon reciept")]
	UponReciept
}