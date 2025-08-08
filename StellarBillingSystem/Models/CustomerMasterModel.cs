using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class CustomerMasterModel
{
	public CustomerMasterModel()
	{
	}

	private int strCustomerID;
	private String strCustomerName;
	private String strDateofBirth;
	private String strGender;
	private String strAddress;
	private String strCity;
	private String strMobileNumber;
	
    private bool strIsDelete;
    private String strLastUpdatedDate;
    private String strLastUpdatedUser;
    private String strLastUpdatedmachine;
    private string branchID;

    private string fathername;
    private string state;
    private string country;
    private string pincode;


    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CustomerID { get => strCustomerID; set => strCustomerID = value; }

    [MaxLength(100)]
    public string? CustomerName { get => strCustomerName; set => strCustomerName = value; }

    [MaxLength(50)]
    public string? DateofBirth { get => strDateofBirth; set => strDateofBirth = value; }

    [MaxLength(50)]
    public string? Gender { get => strGender; set => strGender = value; }

    [MaxLength(100)]
    public string? Address { get => strAddress; set => strAddress = value; }

    [MaxLength(50)]
    public string? City { get => strCity; set => strCity = value; }

    [MaxLength(20)]
    public string? MobileNumber { get => strMobileNumber; set => strMobileNumber = value; }

    [MaxLength(30)]
    public string? LastUpdatedDate { get => strLastUpdatedDate; set => strLastUpdatedDate = value; }
    [MaxLength(30)]
    public string? LastUpdatedUser { get => strLastUpdatedUser; set => strLastUpdatedUser = value; }
    [MaxLength(30)]
    public string? LastUpdatedmachine { get => strLastUpdatedmachine; set => strLastUpdatedmachine = value; }
    public bool IsDelete { get => strIsDelete; set => strIsDelete = value; }
    [MaxLength(100)]
    public string BranchID { get => branchID; set => branchID = value; }

    [MaxLength(50)]
    public string? Fathername { get => fathername; set => fathername = value; }

    [MaxLength(30)]
    public string? State { get => state; set => state = value; }

    [MaxLength(30)]
    public string? Country { get => country; set => country = value; }

    [MaxLength(10)]
    public string? Pincode { get => pincode; set => pincode = value; }
}
