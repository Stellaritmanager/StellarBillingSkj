﻿using System;

public class VoucherCustomerDetailModel
{
	public VoucherCustomerDetailModel()
	{
	}

	private String strVoucherID;
    private String strCustomerID;
    private String strExpiryDate;
    private String strLastUpdatedDate;
    private String strLastUpdatedUser;
    private String strLastUpdatedmachine;
    private string branchID;

    public string VoucherID { get => strVoucherID; set => strVoucherID = value; }
    public string? CustomerID { get => strCustomerID; set => strCustomerID = value; }
    public string? ExpiryDate { get => strExpiryDate; set => strExpiryDate = value; }
    public string? LastUpdatedDate { get => strLastUpdatedDate; set => strLastUpdatedDate = value; }
    public string? LastUpdatedUser { get => strLastUpdatedUser; set => strLastUpdatedUser = value; }
    public string? LastUpdatedmachine { get => strLastUpdatedmachine; set => strLastUpdatedmachine = value; }
    public string BranchID { get => branchID; set => branchID = value; }
}
