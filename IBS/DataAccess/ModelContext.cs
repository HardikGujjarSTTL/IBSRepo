using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace IBS.DataAccess;

public partial class ModelContext : DbContext
{
    public ModelContext()
    {
    }

    public ModelContext(DbContextOptions<ModelContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Abc> Abcs { get; set; }

    public virtual DbSet<AuCri> AuCris { get; set; }

    public virtual DbSet<AuditT20Ic> AuditT20Ics { get; set; }

    public virtual DbSet<BackupT05Vendor02oct2010> BackupT05Vendor02oct2010s { get; set; }

    public virtual DbSet<BounceT25RvDetail> BounceT25RvDetails { get; set; }

    public virtual DbSet<CallLetter> CallLetters { get; set; }

    public virtual DbSet<CallsmarkedtoieView> CallsmarkedtoieViews { get; set; }

    public virtual DbSet<CheckCall> CheckCalls { get; set; }

    public virtual DbSet<CheckPo> CheckPos { get; set; }

    public virtual DbSet<ClientFeedback> ClientFeedbacks { get; set; }

    public virtual DbSet<CrisPymtDtl> CrisPymtDtls { get; set; }

    public virtual DbSet<Deowork> Deoworks { get; set; }

    public virtual DbSet<DocumentCatalogView> DocumentCatalogViews { get; set; }

    public virtual DbSet<ErpProblem> ErpProblems { get; set; }

    public virtual DbSet<GeneralFile> GeneralFiles { get; set; }

    public virtual DbSet<Generatevoucher> Generatevouchers { get; set; }

    public virtual DbSet<Gf> Gfs { get; set; }

    public virtual DbSet<HistT06Consignee> HistT06Consignees { get; set; }

    public virtual DbSet<HistT12BillPayingOfficer> HistT12BillPayingOfficers { get; set; }

    public virtual DbSet<IbsAppdocument> IbsAppdocuments { get; set; }

    public virtual DbSet<IbsDocument> IbsDocuments { get; set; }

    public virtual DbSet<IbsDocumentcategory> IbsDocumentcategories { get; set; }

    public virtual DbSet<IbsSapIntegration> IbsSapIntegrations { get; set; }

    public virtual DbSet<IbsToSapInvoice> IbsToSapInvoices { get; set; }

    public virtual DbSet<IbsToSapInvoice01> IbsToSapInvoice01s { get; set; }

    public virtual DbSet<IbsToSapInvoiceCnote> IbsToSapInvoiceCnotes { get; set; }

    public virtual DbSet<IbsToSapInvoiceCnoteExt> IbsToSapInvoiceCnoteExts { get; set; }

    public virtual DbSet<IbsToSapInvoiceExt> IbsToSapInvoiceExts { get; set; }

    public virtual DbSet<IbsToSapInvoiceExt01> IbsToSapInvoiceExt01s { get; set; }

    public virtual DbSet<IbslabToSapInvoice> IbslabToSapInvoices { get; set; }

    public virtual DbSet<IbslabToSapInvoiceExt> IbslabToSapInvoiceExts { get; set; }

    public virtual DbSet<IcIntermediate> IcIntermediates { get; set; }

    public virtual DbSet<IcPoAmendment> IcPoAmendments { get; set; }

    public virtual DbSet<IcVisit> IcVisits { get; set; }

    public virtual DbSet<IeCallLocationEntryexit> IeCallLocationEntryexits { get; set; }

    public virtual DbSet<IePlanCallTime> IePlanCallTimes { get; set; }

    public virtual DbSet<IeStamp> IeStamps { get; set; }

    public virtual DbSet<ImmsRitesPoDetail> ImmsRitesPoDetails { get; set; }

    public virtual DbSet<ImmsRitesPoHdr> ImmsRitesPoHdrs { get; set; }

    public virtual DbSet<ImmsRitesPocaDtl> ImmsRitesPocaDtls { get; set; }

    public virtual DbSet<ImmsRitesPocaHdr> ImmsRitesPocaHdrs { get; set; }

    public virtual DbSet<ImpSd142175045> ImpSd142175045s { get; set; }

    public virtual DbSet<ImpSd148175452> ImpSd148175452s { get; set; }

    public virtual DbSet<IndiaPinCode> IndiaPinCodes { get; set; }

    public virtual DbSet<InspectionTestParamvalue> InspectionTestParamvalues { get; set; }

    public virtual DbSet<InspectionTestPln> InspectionTestPlns { get; set; }

    public virtual DbSet<InspectionTestPlnTran> InspectionTestPlnTrans { get; set; }

    public virtual DbSet<InspectionTestPlndocVerify> InspectionTestPlndocVerifies { get; set; }

    public virtual DbSet<InspectionTstPlnDimension> InspectionTstPlnDimensions { get; set; }

    public virtual DbSet<InspectionTstPlndim> InspectionTstPlndims { get; set; }

    public virtual DbSet<InspectionTstPlndimTran> InspectionTstPlndimTrans { get; set; }

    public virtual DbSet<InspectionTstPlndimension1> InspectionTstPlndimensions1 { get; set; }

    public virtual DbSet<Mastertablestatus> Mastertablestatuses { get; set; }

    public virtual DbSet<Migration> Migrations { get; set; }

    public virtual DbSet<MmpPomaDtl> MmpPomaDtls { get; set; }

    public virtual DbSet<MmpPomaHdr> MmpPomaHdrs { get; set; }

    public virtual DbSet<Myview> Myviews { get; set; }

    public virtual DbSet<NoIeWorkPlan> NoIeWorkPlans { get; set; }

    public virtual DbSet<OauthAccessToken> OauthAccessTokens { get; set; }

    public virtual DbSet<OauthAuthCode> OauthAuthCodes { get; set; }

    public virtual DbSet<OauthClient> OauthClients { get; set; }

    public virtual DbSet<OauthPersonalAccessClient> OauthPersonalAccessClients { get; set; }

    public virtual DbSet<OauthRefreshToken> OauthRefreshTokens { get; set; }

    public virtual DbSet<OldSystemOutstanding> OldSystemOutstandings { get; set; }

    public virtual DbSet<OngoingNonrlyContract> OngoingNonrlyContracts { get; set; }

    public virtual DbSet<OnlinePayment> OnlinePayments { get; set; }

    public virtual DbSet<PasswordReset> PasswordResets { get; set; }

    public virtual DbSet<R25R26> R25R26s { get; set; }

    public virtual DbSet<R29> R29s { get; set; }

    public virtual DbSet<ReturnedBillsBpoChange> ReturnedBillsBpoChanges { get; set; }

    public virtual DbSet<RitesBillDtl> RitesBillDtls { get; set; }

    public virtual DbSet<Rkm02> Rkm02s { get; set; }

    public virtual DbSet<RkmT26ChequePosting> RkmT26ChequePostings { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<RptPrmIcLine> RptPrmIcLines { get; set; }

    public virtual DbSet<RptPrmInspectionCertificate> RptPrmInspectionCertificates { get; set; }

    public virtual DbSet<T01Region> T01Regions { get; set; }

    public virtual DbSet<T02User> T02Users { get; set; }

    public virtual DbSet<T03City> T03Cities { get; set; }

    public virtual DbSet<T04Uom> T04Uoms { get; set; }

    public virtual DbSet<T05Vendor> T05Vendors { get; set; }

    public virtual DbSet<T06Code> T06Codes { get; set; }

    public virtual DbSet<T06Consignee> T06Consignees { get; set; }

    public virtual DbSet<T07RitesDesig> T07RitesDesigs { get; set; }

    public virtual DbSet<T08IeControllOfficer> T08IeControllOfficers { get; set; }

    public virtual DbSet<T09Ie> T09Ies { get; set; }

    public virtual DbSet<T100VenderCluster> T100VenderClusters { get; set; }

    public virtual DbSet<T101IeCluster> T101IeClusters { get; set; }

    public virtual DbSet<T102IeMaximumCallLimit> T102IeMaximumCallLimits { get; set; }

    public virtual DbSet<T103VendDoc> T103VendDocs { get; set; }

    public virtual DbSet<T104VendEquipClbrCert> T104VendEquipClbrCerts { get; set; }

    public virtual DbSet<T105LoLogin> T105LoLogins { get; set; }

    public virtual DbSet<T106LoOrgn> T106LoOrgns { get; set; }

    public virtual DbSet<T107LoLogginLog> T107LoLogginLogs { get; set; }

    public virtual DbSet<T108RemarkedCall> T108RemarkedCalls { get; set; }

    public virtual DbSet<T109LabSampleInfo> T109LabSampleInfos { get; set; }

    public virtual DbSet<T10IcBookset> T10IcBooksets { get; set; }

    public virtual DbSet<T110LabDoc> T110LabDocs { get; set; }

    public virtual DbSet<T11CallCancelCode> T11CallCancelCodes { get; set; }

    public virtual DbSet<T12BillPayingOfficer> T12BillPayingOfficers { get; set; }

    public virtual DbSet<T13PoMaster> T13PoMasters { get; set; }

    public virtual DbSet<T13T15View> T13T15Views { get; set; }

    public virtual DbSet<T14PoBpo> T14PoBpos { get; set; }

    public virtual DbSet<T14aPoNonrly> T14aPoNonrlies { get; set; }

    public virtual DbSet<T15PoDetail> T15PoDetails { get; set; }

    public virtual DbSet<T16IcCancel> T16IcCancels { get; set; }

    public virtual DbSet<T17CallRegister> T17CallRegisters { get; set; }

    public virtual DbSet<T17CallRegisterSearchView> T17CallRegisterSearchViews { get; set; }

    public virtual DbSet<T18CallDetail> T18CallDetails { get; set; }

    public virtual DbSet<T19CallCancel> T19CallCancels { get; set; }

    public virtual DbSet<T20Ic> T20Ics { get; set; }

    public virtual DbSet<T21CallStatusCode> T21CallStatusCodes { get; set; }

    public virtual DbSet<T22Bill> T22Bills { get; set; }

    public virtual DbSet<T23BillItem> T23BillItems { get; set; }

    public virtual DbSet<T24Rv> T24Rvs { get; set; }

    public virtual DbSet<T252709> T252709s { get; set; }

    public virtual DbSet<T25RvDetail> T25RvDetails { get; set; }

    public virtual DbSet<T26ChequePosting> T26ChequePostings { get; set; }

    public virtual DbSet<T27Jv> T27Jvs { get; set; }

    public virtual DbSet<T28SapRealisation> T28SapRealisations { get; set; }

    public virtual DbSet<T29JvDetail> T29JvDetails { get; set; }

    public virtual DbSet<T30IcReceived> T30IcReceiveds { get; set; }

    public virtual DbSet<T31HologramIssued> T31HologramIssueds { get; set; }

    public virtual DbSet<T32ClientLogin> T32ClientLogins { get; set; }

    public virtual DbSet<T33HologramAccountal> T33HologramAccountals { get; set; }

    public virtual DbSet<T34RailPrice> T34RailPrices { get; set; }

    public virtual DbSet<T35RailPriceDetail> T35RailPriceDetails { get; set; }

    public virtual DbSet<T36Bill> T36Bills { get; set; }

    public virtual DbSet<T37ClientLogginLog> T37ClientLogginLogs { get; set; }

    public virtual DbSet<T38DefectCode> T38DefectCodes { get; set; }

    public virtual DbSet<T39JiStatusCode> T39JiStatusCodes { get; set; }

    public virtual DbSet<T40ConsigneeComplaint> T40ConsigneeComplaints { get; set; }

    public virtual DbSet<T41NcMaster> T41NcMasters { get; set; }

    public virtual DbSet<T42NcDetail> T42NcDetails { get; set; }

    public virtual DbSet<T43CallReturn> T43CallReturns { get; set; }

    public virtual DbSet<T44SuperSurprise> T44SuperSurprises { get; set; }

    public virtual DbSet<T45ClaimMaster> T45ClaimMasters { get; set; }

    public virtual DbSet<T46ClaimDetail> T46ClaimDetails { get; set; }

    public virtual DbSet<T47IeWorkPlan> T47IeWorkPlans { get; set; }

    public virtual DbSet<T48NiIeWorkPlan> T48NiIeWorkPlans { get; set; }

    public virtual DbSet<T49IcPhotoEnclosed> T49IcPhotoEncloseds { get; set; }

    public virtual DbSet<T50LabRegister> T50LabRegisters { get; set; }

    public virtual DbSet<T51LabRegisterDetail> T51LabRegisterDetails { get; set; }

    public virtual DbSet<T52LabPosting> T52LabPostings { get; set; }

    public virtual DbSet<T53VigilanceCasesMaster> T53VigilanceCasesMasters { get; set; }

    public virtual DbSet<T54VigilanceCasesDetail> T54VigilanceCasesDetails { get; set; }

    public virtual DbSet<T55LabInvoice> T55LabInvoices { get; set; }

    public virtual DbSet<T56LabPayment> T56LabPayments { get; set; }

    public virtual DbSet<T57OngoingContract> T57OngoingContracts { get; set; }

    public virtual DbSet<T58ClientContact> T58ClientContacts { get; set; }

    public virtual DbSet<T59LabExp> T59LabExps { get; set; }

    public virtual DbSet<T60IePoiMapping> T60IePoiMappings { get; set; }

    public virtual DbSet<T61ItemMaster> T61ItemMasters { get; set; }

    public virtual DbSet<T62MasterItemPlno> T62MasterItemPlnos { get; set; }

    public virtual DbSet<T63Exp> T63Exps { get; set; }

    public virtual DbSet<T64TestCategory> T64TestCategories { get; set; }

    public virtual DbSet<T65LaboratoryMaster> T65LaboratoryMasters { get; set; }

    public virtual DbSet<T65LaboratoryMasterBak> T65LaboratoryMasterBaks { get; set; }

    public virtual DbSet<T65LaboratoryMasterBak1> T65LaboratoryMasterBaks1 { get; set; }

    public virtual DbSet<T66TechRef> T66TechRefs { get; set; }

    public virtual DbSet<T67Highlight> T67Highlights { get; set; }

    public virtual DbSet<T68SealingPatternCode> T68SealingPatternCodes { get; set; }

    public virtual DbSet<T69NcCode> T69NcCodes { get; set; }

    public virtual DbSet<T70UnregisteredCall> T70UnregisteredCalls { get; set; }

    public virtual DbSet<T71RcfBill> T71RcfBills { get; set; }

    public virtual DbSet<T71RcfBills16apr2010> T71RcfBills16apr2010s { get; set; }

    public virtual DbSet<T72IeMessage> T72IeMessages { get; set; }

    public virtual DbSet<T73PayingWindow> T73PayingWindows { get; set; }

    public virtual DbSet<T74ChecksheetCatalog> T74ChecksheetCatalogs { get; set; }

    public virtual DbSet<T74DocumentType> T74DocumentTypes { get; set; }

    public virtual DbSet<T75DocSubType> T75DocSubTypes { get; set; }

    public virtual DbSet<T76DocumentCatalog> T76DocumentCatalogs { get; set; }

    public virtual DbSet<T77IcfBill> T77IcfBills { get; set; }

    public virtual DbSet<T78CentralQoi> T78CentralQois { get; set; }

    public virtual DbSet<T79CentralQoinsp> T79CentralQoinsps { get; set; }

    public virtual DbSet<T80PoMaster> T80PoMasters { get; set; }

    public virtual DbSet<T81CrRej> T81CrRejs { get; set; }

    public virtual DbSet<T82PoDetail> T82PoDetails { get; set; }

    public virtual DbSet<T83BeTarget> T83BeTargets { get; set; }

    public virtual DbSet<T84OutsLy> T84OutsLies { get; set; }

    public virtual DbSet<T85BillingAdjustementPcdo> T85BillingAdjustementPcdos { get; set; }

    public virtual DbSet<T86LabInvoiceDetail> T86LabInvoiceDetails { get; set; }

    public virtual DbSet<T87BillControl> T87BillControls { get; set; }

    public virtual DbSet<T88SapBilling> T88SapBillings { get; set; }

    public virtual DbSet<T89Gst> T89Gsts { get; set; }

    public virtual DbSet<T90RlyDesignation> T90RlyDesignations { get; set; }

    public virtual DbSet<T91Railway> T91Railways { get; set; }

    public virtual DbSet<T92State> T92States { get; set; }

    public virtual DbSet<T93IcType> T93IcTypes { get; set; }

    public virtual DbSet<T94Bank> T94Banks { get; set; }

    public virtual DbSet<T95AccountCode> T95AccountCodes { get; set; }

    public virtual DbSet<T96Message> T96Messages { get; set; }

    public virtual DbSet<T97ControlFile> T97ControlFiles { get; set; }

    public virtual DbSet<T98Servicetax> T98Servicetaxes { get; set; }

    public virtual DbSet<T99ClusterMaster> T99ClusterMasters { get; set; }

    public virtual DbSet<Tblexception> Tblexceptions { get; set; }

    public virtual DbSet<Temp10Ic> Temp10Ics { get; set; }

    public virtual DbSet<Temp22Bill> Temp22Bills { get; set; }

    public virtual DbSet<Temp2425Realisation> Temp2425Realisations { get; set; }

    public virtual DbSet<Temp25RvDetail> Temp25RvDetails { get; set; }

    public virtual DbSet<TempDeptWiseCall> TempDeptWiseCalls { get; set; }

    public virtual DbSet<TempOnlineComplaint> TempOnlineComplaints { get; set; }

    public virtual DbSet<TempVend> TempVends { get; set; }

    public virtual DbSet<TraineeEmployeeMaster> TraineeEmployeeMasters { get; set; }

    public virtual DbSet<TrainingCourseMaster> TrainingCourseMasters { get; set; }

    public virtual DbSet<TrainingDetail> TrainingDetails { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<V05Vendor> V05Vendors { get; set; }

    public virtual DbSet<V06Consignee> V06Consignees { get; set; }

    public virtual DbSet<V12BillPayingOfficer> V12BillPayingOfficers { get; set; }

    public virtual DbSet<V17IeWiseDailyCallSummary> V17IeWiseDailyCallSummaries { get; set; }

    public virtual DbSet<V20Ic> V20Ics { get; set; }

    public virtual DbSet<V22Bill> V22Bills { get; set; }

    public virtual DbSet<V22aBillingSummary> V22aBillingSummaries { get; set; }

    public virtual DbSet<V22bOutstandingBill> V22bOutstandingBills { get; set; }

    public virtual DbSet<V23BillItem> V23BillItems { get; set; }

    public virtual DbSet<V2425Realisation> V2425Realisations { get; set; }

    public virtual DbSet<V252709> V252709s { get; set; }

    public virtual DbSet<V25InspectionDetail> V25InspectionDetails { get; set; }

    public virtual DbSet<V40ConsigneeComplaint> V40ConsigneeComplaints { get; set; }

    public virtual DbSet<V55LabInvoice> V55LabInvoices { get; set; }

    public virtual DbSet<V56LabInvoiceDetail> V56LabInvoiceDetails { get; set; }

    public virtual DbSet<VTractionDistribution> VTractionDistributions { get; set; }

    public virtual DbSet<VendPoMa> VendPoMas { get; set; }

    public virtual DbSet<VendPoMaDetail> VendPoMaDetails { get; set; }

    public virtual DbSet<VendPoMaMaster> VendPoMaMasters { get; set; }

    public virtual DbSet<VenderCallRegisterItemView1> VenderCallRegisterItemView1s { get; set; }

    public virtual DbSet<VenderCallRegisterItemView2> VenderCallRegisterItemView2s { get; set; }

    public virtual DbSet<VendorCallPoDetailsView> VendorCallPoDetailsViews { get; set; }

    public virtual DbSet<VendorFeedback> VendorFeedbacks { get; set; }

    public virtual DbSet<ViewGetmanufvend> ViewGetmanufvends { get; set; }

    public virtual DbSet<ViewGetvendor> ViewGetvendors { get; set; }

    public virtual DbSet<ViewLaboratory> ViewLaboratories { get; set; }

    public virtual DbSet<ViewPomasterlist> ViewPomasterlists { get; set; }

    public virtual DbSet<ViewVoucherList> ViewVoucherLists { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseOracle("Data Source=(DESCRIPTION=(ADDRESS_LIST= (ADDRESS=(COMMUNITY=tcpcom.world)(PROTOCOL=tcp)(HOST=192.168.0.215)(PORT=1521)))(CONNECT_DATA=(SID=orcl))); User ID=IBSDev;Password=IBSDev");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasDefaultSchema("IBSDEV")
            .UseCollation("USING_NLS_COMP");

        modelBuilder.Entity<Abc>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("ABC");

            entity.Property(e => e.BillNo)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("BILL_NO");
            entity.Property(e => e.CallRecvDt)
                .HasColumnType("DATE")
                .HasColumnName("CALL_RECV_DT");
            entity.Property(e => e.CallSno)
                .HasPrecision(5)
                .HasColumnName("CALL_SNO");
            entity.Property(e => e.CallStatus)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CALL_STATUS");
            entity.Property(e => e.CaseNo)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CASE_NO");
            entity.Property(e => e.IcTypeId)
                .HasPrecision(1)
                .HasColumnName("IC_TYPE_ID");
        });

        modelBuilder.Entity<AuCri>(entity =>
        {
            entity.HasKey(e => e.Au).HasName("AU_CRIS_PK");

            entity.ToTable("AU_CRIS");

            entity.Property(e => e.Au)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("AU");
            entity.Property(e => e.Address)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("ADDRESS");
            entity.Property(e => e.Audesc)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("AUDESC");
            entity.Property(e => e.RlyCd)
                .HasMaxLength(7)
                .IsUnicode(false)
                .HasColumnName("RLY_CD");
        });

        modelBuilder.Entity<AuditT20Ic>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("AUDIT_T20_IC");

            entity.Property(e => e.BillNo)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("BILL_NO");
            entity.Property(e => e.BkNo)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("BK_NO");
            entity.Property(e => e.Job)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("JOB");
            entity.Property(e => e.Proc)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("PROC");
            entity.Property(e => e.Seq)
                .HasColumnType("NUMBER")
                .HasColumnName("SEQ");
            entity.Property(e => e.SetNo)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("SET_NO");
            entity.Property(e => e.Term)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("TERM");
            entity.Property(e => e.TimeNow)
                .HasColumnType("DATE")
                .HasColumnName("TIME_NOW");
            entity.Property(e => e.UserAt)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("USER_AT");
        });

        modelBuilder.Entity<BackupT05Vendor02oct2010>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("BACKUP_T05_VENDOR_02OCT2010");

            entity.Property(e => e.Datetime)
                .HasColumnType("DATE")
                .HasColumnName("DATETIME");
            entity.Property(e => e.UserId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("USER_ID");
            entity.Property(e => e.VendAdd1)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("VEND_ADD1");
            entity.Property(e => e.VendAdd2)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("VEND_ADD2");
            entity.Property(e => e.VendApproval)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("VEND_APPROVAL");
            entity.Property(e => e.VendApprovalFr)
                .HasColumnType("DATE")
                .HasColumnName("VEND_APPROVAL_FR");
            entity.Property(e => e.VendApprovalTo)
                .HasColumnType("DATE")
                .HasColumnName("VEND_APPROVAL_TO");
            entity.Property(e => e.VendCd)
                .HasPrecision(6)
                .HasColumnName("VEND_CD");
            entity.Property(e => e.VendCdAlpha)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("VEND_CD_ALPHA");
            entity.Property(e => e.VendCityCd)
                .HasPrecision(4)
                .HasColumnName("VEND_CITY_CD");
            entity.Property(e => e.VendContactPer1)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("VEND_CONTACT_PER_1");
            entity.Property(e => e.VendContactPer2)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("VEND_CONTACT_PER_2");
            entity.Property(e => e.VendContactTel1)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("VEND_CONTACT_TEL_1");
            entity.Property(e => e.VendContactTel2)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("VEND_CONTACT_TEL_2");
            entity.Property(e => e.VendEmail)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("VEND_EMAIL");
            entity.Property(e => e.VendName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("VEND_NAME");
            entity.Property(e => e.VendRemarks)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .HasColumnName("VEND_REMARKS");
            entity.Property(e => e.VendStatus)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("VEND_STATUS");
            entity.Property(e => e.VendStatusDtFr)
                .HasColumnType("DATE")
                .HasColumnName("VEND_STATUS_DT_FR");
            entity.Property(e => e.VendStatusDtTo)
                .HasColumnType("DATE")
                .HasColumnName("VEND_STATUS_DT_TO");
        });

        modelBuilder.Entity<BounceT25RvDetail>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("BOUNCE_T25_RV_DETAILS");

            entity.Property(e => e.AccCd)
                .HasPrecision(4)
                .HasColumnName("ACC_CD");
            entity.Property(e => e.Amount)
                .HasColumnType("NUMBER(12,2)")
                .HasColumnName("AMOUNT");
            entity.Property(e => e.BankCd)
                .HasPrecision(3)
                .HasColumnName("BANK_CD");
            entity.Property(e => e.BounceAmt)
                .HasColumnType("NUMBER(12,2)")
                .HasColumnName("BOUNCE_AMT");
            entity.Property(e => e.ChqDt)
                .HasColumnType("DATE")
                .HasColumnName("CHQ_DT");
            entity.Property(e => e.ChqNo)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("CHQ_NO");
            entity.Property(e => e.Region)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("REGION");
        });

        modelBuilder.Entity<CallLetter>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("CALL_LETTER");

            entity.Property(e => e.ClCh)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("CL_CH");
            entity.Property(e => e.ClCo)
                .HasMaxLength(52)
                .IsUnicode(false)
                .HasColumnName("CL_CO");
            entity.Property(e => e.ClCp)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("CL_CP");
            entity.Property(e => e.ClCs)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("CL_CS");
            entity.Property(e => e.ClDt)
                .HasColumnType("DATE")
                .HasColumnName("CL_DT");
            entity.Property(e => e.ClEn)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("CL_EN");
            entity.Property(e => e.ClIt)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("CL_IT");
            entity.Property(e => e.ClMp)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("CL_MP");
            entity.Property(e => e.ClMph)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("CL_MPH");
            entity.Property(e => e.ClPd)
                .HasColumnType("DATE")
                .HasColumnName("CL_PD");
            entity.Property(e => e.ClPh)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("CL_PH");
            entity.Property(e => e.ClPo)
                .HasMaxLength(75)
                .IsUnicode(false)
                .HasColumnName("CL_PO");
            entity.Property(e => e.ClPr)
                .HasMaxLength(115)
                .IsUnicode(false)
                .HasColumnName("CL_PR");
            entity.Property(e => e.ClRmk)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("CL_RMK");
            entity.Property(e => e.ClSn)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("CL_SN");
            entity.Property(e => e.ClSt)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("CL_ST");
            entity.Property(e => e.ClTy)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CL_TY");
            entity.Property(e => e.ClVn)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("CL_VN");
        });

        modelBuilder.Entity<CallsmarkedtoieView>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("CALLSMARKEDTOIE_VIEW");

            entity.Property(e => e.CallMarkDt)
                .HasColumnType("DATE")
                .HasColumnName("CALL_MARK_DT");
            entity.Property(e => e.CallRecvDt)
                .HasColumnType("DATE")
                .HasColumnName("CALL_RECV_DT");
            entity.Property(e => e.CallSno)
                .HasPrecision(5)
                .HasColumnName("CALL_SNO");
            entity.Property(e => e.CallStatus)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CALL_STATUS");
            entity.Property(e => e.CallStatusFull)
                .HasMaxLength(108)
                .IsUnicode(false)
                .HasColumnName("CALL_STATUS_FULL");
            entity.Property(e => e.CaseNo)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CASE_NO");
            entity.Property(e => e.Colour)
                .HasMaxLength(7)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("COLOUR");
            entity.Property(e => e.Consignee)
                .HasMaxLength(116)
                .IsUnicode(false)
                .HasColumnName("CONSIGNEE");
            entity.Property(e => e.Count)
                .HasColumnType("NUMBER")
                .HasColumnName("COUNT");
            entity.Property(e => e.Datetime)
                .HasColumnType("DATE")
                .HasColumnName("DATETIME");
            entity.Property(e => e.DocsSubmitted)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("DOCS_SUBMITTED");
            entity.Property(e => e.DtInspDesire)
                .HasColumnType("DATE")
                .HasColumnName("DT_INSP_DESIRE");
            entity.Property(e => e.ExtDelvDt)
                .HasColumnType("DATE")
                .HasColumnName("EXT_DELV_DT");
            entity.Property(e => e.IeCd)
                .HasPrecision(6)
                .HasColumnName("IE_CD");
            entity.Property(e => e.IeName)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("IE_NAME");
            entity.Property(e => e.IePhoneNo)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("IE_PHONE_NO");
            entity.Property(e => e.ItemDescPo)
                .HasMaxLength(400)
                .IsUnicode(false)
                .HasColumnName("ITEM_DESC_PO");
            entity.Property(e => e.Manufacturer)
                .HasMaxLength(151)
                .IsUnicode(false)
                .HasColumnName("MANUFACTURER");
            entity.Property(e => e.MfgCd)
                .HasPrecision(6)
                .HasColumnName("MFG_CD");
            entity.Property(e => e.MfgPers)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("MFG_PERS");
            entity.Property(e => e.MfgPhone)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("MFG_PHONE");
            entity.Property(e => e.NewVendor)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("NEW_VENDOR");
            entity.Property(e => e.PoDt)
                .HasColumnType("DATE")
                .HasColumnName("PO_DT");
            entity.Property(e => e.PoNo)
                .HasMaxLength(75)
                .IsUnicode(false)
                .HasColumnName("PO_NO");
            entity.Property(e => e.PoSource)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("PO_SOURCE");
            entity.Property(e => e.PoYr)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("PO_YR");
            entity.Property(e => e.Remarks)
                .HasMaxLength(400)
                .IsUnicode(false)
                .HasColumnName("REMARKS");
            entity.Property(e => e.RlyCd)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("RLY_CD");
            entity.Property(e => e.Source)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("SOURCE");
            entity.Property(e => e.UserId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("USER_ID");
            entity.Property(e => e.VendCd)
                .HasPrecision(6)
                .HasColumnName("VEND_CD");
            entity.Property(e => e.Vendor)
                .HasMaxLength(151)
                .IsUnicode(false)
                .HasColumnName("VENDOR");
        });

        modelBuilder.Entity<CheckCall>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("CHECK_CALL");

            entity.Property(e => e.BkNo)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("BK_NO");
            entity.Property(e => e.CallRecvDt)
                .HasColumnType("DATE")
                .HasColumnName("CALL_RECV_DT");
            entity.Property(e => e.CallSno)
                .HasPrecision(5)
                .HasColumnName("CALL_SNO");
            entity.Property(e => e.CallStatus)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CALL_STATUS");
            entity.Property(e => e.CaseNo)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CASE_NO");
            entity.Property(e => e.ExtDelvDt)
                .HasColumnType("DATE")
                .HasColumnName("EXT_DELV_DT");
            entity.Property(e => e.IcDt)
                .HasColumnType("DATE")
                .HasColumnName("IC_DT");
            entity.Property(e => e.IcNo)
                .HasMaxLength(29)
                .IsUnicode(false)
                .HasColumnName("IC_NO");
            entity.Property(e => e.IeName)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("IE_NAME");
            entity.Property(e => e.IePhoneNo)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("IE_PHONE_NO");
            entity.Property(e => e.PendingCharges)
                .HasPrecision(2)
                .HasColumnName("PENDING_CHARGES");
            entity.Property(e => e.RlyCd)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("RLY_CD");
            entity.Property(e => e.SetNo)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("SET_NO");
            entity.Property(e => e.VendCd)
                .HasPrecision(6)
                .HasColumnName("VEND_CD");
            entity.Property(e => e.VendContactTel1)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("VEND_CONTACT_TEL_1");
        });

        modelBuilder.Entity<CheckPo>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("CHECK_PO");

            entity.Property(e => e.CaseNo)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CASE_NO");
            entity.Property(e => e.ExtDelvDt)
                .HasColumnType("DATE")
                .HasColumnName("EXT_DELV_DT");
            entity.Property(e => e.PoDt)
                .HasColumnType("DATE")
                .HasColumnName("PO_DT");
            entity.Property(e => e.PoNo)
                .HasMaxLength(75)
                .IsUnicode(false)
                .HasColumnName("PO_NO");
            entity.Property(e => e.PoOrLetter)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("PO_OR_LETTER");
            entity.Property(e => e.RefNo)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("REF_NO");
            entity.Property(e => e.RlyCd)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("RLY_CD");
            entity.Property(e => e.VendCd)
                .HasPrecision(6)
                .HasColumnName("VEND_CD");
            entity.Property(e => e.VendContactTel1)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("VEND_CONTACT_TEL_1");
        });

        modelBuilder.Entity<ClientFeedback>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("CLIENT_FEEDBACK");

            entity.HasIndex(e => new { e.Mobile, e.RegionCode }, "UK_CLIENT_FEEDBACK").IsUnique();

            entity.Property(e => e.Client)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("CLIENT");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.Field1)
                .HasPrecision(1)
                .HasColumnName("FIELD_1");
            entity.Property(e => e.Field10)
                .HasPrecision(1)
                .HasColumnName("FIELD_10");
            entity.Property(e => e.Field11)
                .HasPrecision(1)
                .HasColumnName("FIELD_11");
            entity.Property(e => e.Field12)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("FIELD_12");
            entity.Property(e => e.Field2)
                .HasPrecision(1)
                .HasColumnName("FIELD_2");
            entity.Property(e => e.Field3)
                .HasPrecision(1)
                .HasColumnName("FIELD_3");
            entity.Property(e => e.Field4)
                .HasPrecision(1)
                .HasColumnName("FIELD_4");
            entity.Property(e => e.Field5)
                .HasPrecision(1)
                .HasColumnName("FIELD_5");
            entity.Property(e => e.Field6)
                .HasPrecision(1)
                .HasColumnName("FIELD_6");
            entity.Property(e => e.Field7)
                .HasPrecision(1)
                .HasColumnName("FIELD_7");
            entity.Property(e => e.Field8)
                .HasPrecision(1)
                .HasColumnName("FIELD_8");
            entity.Property(e => e.Field9)
                .HasPrecision(1)
                .HasColumnName("FIELD_9");
            entity.Property(e => e.Mobile)
                .HasPrecision(10)
                .HasColumnName("MOBILE");
            entity.Property(e => e.OffName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("OFF_NAME");
            entity.Property(e => e.RegionCode)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("REGION_CODE");
        });

        modelBuilder.Entity<CrisPymtDtl>(entity =>
        {
            entity.HasKey(e => e.BillNo).HasName("BILL_NO_PK");

            entity.ToTable("CRIS_PYMT_DTLS");

            entity.Property(e => e.BillNo)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("bill_no");
            entity.Property(e => e.Bookdate)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("bookdate");
            entity.Property(e => e.Co6Date)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("co6_date");
            entity.Property(e => e.Co6No)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("co6_no");
            entity.Property(e => e.Co6Status)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("co6_status");
            entity.Property(e => e.Co6StatusDate)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("co6_status_date");
            entity.Property(e => e.Co7Date)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("co7_date");
            entity.Property(e => e.Co7No)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("co7_no");
            entity.Property(e => e.DeductedAmt)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("deducted_amt");
            entity.Property(e => e.IcDt)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("ic_dt");
            entity.Property(e => e.IcNo)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("ic_no");
            entity.Property(e => e.Invoiceno)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("invoiceno");
            entity.Property(e => e.NetAmt)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("net_amt");
            entity.Property(e => e.PassedAmt)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("passed_amt");
            entity.Property(e => e.PaymentDt)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("payment_dt");
            entity.Property(e => e.ReturnDate)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("return_date");
            entity.Property(e => e.ReturnReason)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("return_reason");
            entity.Property(e => e.Status)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("status");
            entity.Property(e => e.Updatedate)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("updatedate");
        });

        modelBuilder.Entity<Deowork>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("DEOWORK");

            entity.Property(e => e.Datetime)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("DATETIME");
            entity.Property(e => e.Poent)
                .HasColumnType("NUMBER")
                .HasColumnName("POENT");
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("USER_NAME");
        });

        modelBuilder.Entity<DocumentCatalogView>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("DOCUMENT_CATALOG_VIEW");

            entity.Property(e => e.DocSubType)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("DOC_SUB_TYPE");
            entity.Property(e => e.DocType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("DOC_TYPE");
            entity.Property(e => e.FId)
                .HasColumnType("NUMBER")
                .HasColumnName("F_ID");
            entity.Property(e => e.FileId)
                .HasMaxLength(7)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("FILE_ID");
        });

        modelBuilder.Entity<ErpProblem>(entity =>
        {
            entity.HasKey(e => e.ErpProbId).HasName("PK_ERP_PROB");

            entity.ToTable("ERP_PROBLEMS");

            entity.Property(e => e.ErpProbId)
                .HasPrecision(6)
                .ValueGeneratedNever()
                .HasColumnName("ERP_PROB_ID");
            entity.Property(e => e.Datetime)
                .HasColumnType("DATE")
                .HasColumnName("DATETIME");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NAME");
            entity.Property(e => e.ProbDesc)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("PROB_DESC");
            entity.Property(e => e.Resolution)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("RESOLUTION");
        });

        modelBuilder.Entity<GeneralFile>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("GENERAL_FILE");

            entity.Property(e => e.AccCb)
                .HasColumnType("NUMBER(12,2)")
                .HasColumnName("ACC_CB");
            entity.Property(e => e.AccCode)
                .HasPrecision(4)
                .HasColumnName("ACC_CODE");
            entity.Property(e => e.AccOb)
                .HasColumnType("NUMBER(12,2)")
                .HasColumnName("ACC_OB");
            entity.Property(e => e.Amount)
                .HasColumnType("NUMBER(12,2)")
                .HasColumnName("AMOUNT");
            entity.Property(e => e.BankCb)
                .HasColumnType("NUMBER(12,2)")
                .HasColumnName("BANK_CB");
            entity.Property(e => e.BankCode)
                .HasPrecision(4)
                .HasColumnName("BANK_CODE");
            entity.Property(e => e.BankOb)
                .HasColumnType("NUMBER(12,2)")
                .HasColumnName("BANK_OB");
            entity.Property(e => e.CheckBy)
                .HasPrecision(2)
                .HasColumnName("CHECK_BY");
            entity.Property(e => e.ChequeNo)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("CHEQUE_NO");
            entity.Property(e => e.CurryCode)
                .HasPrecision(2)
                .HasColumnName("CURRY_CODE");
            entity.Property(e => e.FcurTcDate)
                .HasColumnType("DATE")
                .HasColumnName("FCUR_TC_DATE");
            entity.Property(e => e.Module)
                .HasPrecision(2)
                .HasColumnName("MODULE");
            entity.Property(e => e.Narration)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("NARRATION");
            entity.Property(e => e.PartyName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("PARTY_NAME");
            entity.Property(e => e.PostingDt)
                .HasColumnType("DATE")
                .HasColumnName("POSTING_DT");
            entity.Property(e => e.PostingStatus)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("POSTING_STATUS");
            entity.Property(e => e.PrepBy)
                .HasPrecision(2)
                .HasColumnName("PREP_BY");
            entity.Property(e => e.ProjectCode)
                .HasPrecision(4)
                .HasColumnName("PROJECT_CODE");
            entity.Property(e => e.RefNo)
                .HasPrecision(4)
                .HasColumnName("REF_NO");
            entity.Property(e => e.Region)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("REGION");
            entity.Property(e => e.SbuCode)
                .HasPrecision(2)
                .HasColumnName("SBU_CODE");
            entity.Property(e => e.SnoT25)
                .HasPrecision(4)
                .HasColumnName("SNO_T25");
            entity.Property(e => e.SubCode)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("SUB_CODE");
            entity.Property(e => e.SupplNo)
                .HasPrecision(2)
                .HasColumnName("SUPPL_NO");
            entity.Property(e => e.Tc)
                .HasPrecision(2)
                .HasColumnName("TC");
            entity.Property(e => e.UnitCode)
                .HasPrecision(2)
                .HasColumnName("UNIT_CODE");
            entity.Property(e => e.UserCode)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("USER_CODE");
            entity.Property(e => e.VchrDate)
                .HasColumnType("DATE")
                .HasColumnName("VCHR_DATE");
            entity.Property(e => e.VchrNoT25)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("VCHR_NO_T25");
            entity.Property(e => e.VchrNumb)
                .HasPrecision(4)
                .HasColumnName("VCHR_NUMB");
        });

        modelBuilder.Entity<Generatevoucher>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("GENERATEVOUCHER");

            entity.Property(e => e.VchrNo)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("VCHR_NO");
        });

        modelBuilder.Entity<Gf>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("GF");

            entity.Property(e => e.AccCb)
                .HasColumnType("NUMBER(12,2)")
                .HasColumnName("ACC_CB");
            entity.Property(e => e.AccCode)
                .HasPrecision(4)
                .HasColumnName("ACC_CODE");
            entity.Property(e => e.AccOb)
                .HasColumnType("NUMBER(12,2)")
                .HasColumnName("ACC_OB");
            entity.Property(e => e.Amount)
                .HasColumnType("NUMBER(12,2)")
                .HasColumnName("AMOUNT");
            entity.Property(e => e.BankCb)
                .HasColumnType("NUMBER(12,2)")
                .HasColumnName("BANK_CB");
            entity.Property(e => e.BankCode)
                .HasPrecision(4)
                .HasColumnName("BANK_CODE");
            entity.Property(e => e.BankOb)
                .HasColumnType("NUMBER(12,2)")
                .HasColumnName("BANK_OB");
            entity.Property(e => e.CheckBy)
                .HasPrecision(2)
                .HasColumnName("CHECK_BY");
            entity.Property(e => e.ChequeNo)
                .HasPrecision(6)
                .HasColumnName("CHEQUE_NO");
            entity.Property(e => e.CurryCode)
                .HasPrecision(2)
                .HasColumnName("CURRY_CODE");
            entity.Property(e => e.FcurTcDate)
                .HasColumnType("DATE")
                .HasColumnName("FCUR_TC_DATE");
            entity.Property(e => e.Module)
                .HasPrecision(2)
                .HasColumnName("MODULE");
            entity.Property(e => e.Narration)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("NARRATION");
            entity.Property(e => e.PartyName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("PARTY_NAME");
            entity.Property(e => e.PostingDt)
                .HasColumnType("DATE")
                .HasColumnName("POSTING_DT");
            entity.Property(e => e.PostingStatus)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("POSTING_STATUS");
            entity.Property(e => e.PrepBy)
                .HasPrecision(2)
                .HasColumnName("PREP_BY");
            entity.Property(e => e.ProjectCode)
                .HasPrecision(4)
                .HasColumnName("PROJECT_CODE");
            entity.Property(e => e.RefNo)
                .HasPrecision(4)
                .HasColumnName("REF_NO");
            entity.Property(e => e.Region)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("REGION");
            entity.Property(e => e.SbuCode)
                .HasPrecision(2)
                .HasColumnName("SBU_CODE");
            entity.Property(e => e.SnoT25)
                .HasPrecision(4)
                .HasColumnName("SNO_T25");
            entity.Property(e => e.SubCode)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("SUB_CODE");
            entity.Property(e => e.SupplNo)
                .HasPrecision(2)
                .HasColumnName("SUPPL_NO");
            entity.Property(e => e.Tc)
                .HasPrecision(2)
                .HasColumnName("TC");
            entity.Property(e => e.UnitCode)
                .HasPrecision(2)
                .HasColumnName("UNIT_CODE");
            entity.Property(e => e.UserCode)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("USER_CODE");
            entity.Property(e => e.VchrDate)
                .HasColumnType("DATE")
                .HasColumnName("VCHR_DATE");
            entity.Property(e => e.VchrNoT25)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("VCHR_NO_T25");
            entity.Property(e => e.VchrNumb)
                .HasPrecision(4)
                .HasColumnName("VCHR_NUMB");
        });

        modelBuilder.Entity<HistT06Consignee>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("HIST_T06_CONSIGNEE");

            entity.Property(e => e.ConsigneeAdd1)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("CONSIGNEE_ADD1");
            entity.Property(e => e.ConsigneeAdd2)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("CONSIGNEE_ADD2");
            entity.Property(e => e.ConsigneeCd)
                .HasPrecision(8)
                .HasColumnName("CONSIGNEE_CD");
            entity.Property(e => e.ConsigneeCity)
                .HasPrecision(4)
                .HasColumnName("CONSIGNEE_CITY");
            entity.Property(e => e.ConsigneeDept)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("CONSIGNEE_DEPT");
            entity.Property(e => e.ConsigneeDesig)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("CONSIGNEE_DESIG");
            entity.Property(e => e.ConsigneeFirm)
                .HasMaxLength(75)
                .IsUnicode(false)
                .HasColumnName("CONSIGNEE_FIRM");
            entity.Property(e => e.ConsigneeType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CONSIGNEE_TYPE");
        });

        modelBuilder.Entity<HistT12BillPayingOfficer>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("HIST_T12_BILL_PAYING_OFFICER");

            entity.Property(e => e.BillPassOfficer)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("BILL_PASS_OFFICER");
            entity.Property(e => e.BpoAdd)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("BPO_ADD");
            entity.Property(e => e.BpoAdd1)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("BPO_ADD1");
            entity.Property(e => e.BpoAdd2)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("BPO_ADD2");
            entity.Property(e => e.BpoAdvFlg)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("BPO_ADV_FLG");
            entity.Property(e => e.BpoCd)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("BPO_CD");
            entity.Property(e => e.BpoCdOld)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("BPO_CD_OLD");
            entity.Property(e => e.BpoCityCd)
                .HasPrecision(4)
                .HasColumnName("BPO_CITY_CD");
            entity.Property(e => e.BpoEmail)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("BPO_EMAIL");
            entity.Property(e => e.BpoFax)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("BPO_FAX");
            entity.Property(e => e.BpoFee)
                .HasColumnType("NUMBER(11,4)")
                .HasColumnName("BPO_FEE");
            entity.Property(e => e.BpoFeeType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("BPO_FEE_TYPE");
            entity.Property(e => e.BpoFlg)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("BPO_FLG");
            entity.Property(e => e.BpoIocRegn)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("BPO_IOC_REGN");
            entity.Property(e => e.BpoLocCd)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("BPO_LOC_CD");
            entity.Property(e => e.BpoName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("BPO_NAME");
            entity.Property(e => e.BpoOrgn)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("BPO_ORGN");
            entity.Property(e => e.BpoPhone)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("BPO_PHONE");
            entity.Property(e => e.BpoPin)
                .HasMaxLength(7)
                .IsUnicode(false)
                .HasColumnName("BPO_PIN");
            entity.Property(e => e.BpoRegion)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("BPO_REGION");
            entity.Property(e => e.BpoRly)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("BPO_RLY");
            entity.Property(e => e.BpoState)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("BPO_STATE");
            entity.Property(e => e.BpoTaxType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("BPO_TAX_TYPE");
            entity.Property(e => e.BpoType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("BPO_TYPE");
        });

        modelBuilder.Entity<IbsAppdocument>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("IBS_APPDOCUMENT_PK");

            entity.ToTable("IBS_APPDOCUMENT");

            entity.Property(e => e.Id)
                .HasPrecision(6)
                .HasDefaultValueSql("\"IBSDEV\".\"IBS_APPDOCUMENT_SEQ\".\"NEXTVAL\"")
                .HasColumnName("ID");
            entity.Property(e => e.Accuracy)
                .HasColumnType("NUMBER(18,6)")
                .HasColumnName("ACCURACY");
            entity.Property(e => e.Applicationid)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("APPLICATIONID");
            entity.Property(e => e.Camera)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("CAMERA");
            entity.Property(e => e.Couchdbdocid)
                .HasMaxLength(50)
                .HasColumnName("COUCHDBDOCID");
            entity.Property(e => e.Documentcategory)
                .HasPrecision(6)
                .HasColumnName("DOCUMENTCATEGORY");
            entity.Property(e => e.Documentid)
                .HasPrecision(6)
                .HasColumnName("DOCUMENTID");
            entity.Property(e => e.Extension)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("EXTENSION");
            entity.Property(e => e.Filedisplayname)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("FILEDISPLAYNAME");
            entity.Property(e => e.Fileid)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("FILEID");
            entity.Property(e => e.Isdeleted)
                .HasPrecision(2)
                .HasColumnName("ISDELETED");
            entity.Property(e => e.Isotherdoc)
                .HasPrecision(2)
                .HasColumnName("ISOTHERDOC");
            entity.Property(e => e.Isvideo)
                .HasPrecision(2)
                .HasColumnName("ISVIDEO");
            entity.Property(e => e.Latitude)
                .HasColumnType("NUMBER(18,6)")
                .HasColumnName("LATITUDE");
            entity.Property(e => e.Longitude)
                .HasColumnType("NUMBER(18,6)")
                .HasColumnName("LONGITUDE");
            entity.Property(e => e.Maker)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("MAKER");
            entity.Property(e => e.Otherdocumentname)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("OTHERDOCUMENTNAME");
            entity.Property(e => e.Phototakendate)
                .HasColumnType("DATE")
                .HasColumnName("PHOTOTAKENDATE");
            entity.Property(e => e.Relativepath)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("RELATIVEPATH");
            entity.Property(e => e.Thumnailextension)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("THUMNAILEXTENSION");
            entity.Property(e => e.Thumnailfileid)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("THUMNAILFILEID");
            entity.Property(e => e.Thumnailpath)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("THUMNAILPATH");

            entity.HasOne(d => d.Document).WithMany(p => p.IbsAppdocuments)
                .HasForeignKey(d => d.Documentid)
                .HasConstraintName("FK_GNR_APPDOCUMENT_GNR_DOCUMENT");
        });

        modelBuilder.Entity<IbsDocument>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("IBS_DOCUMENT_PK");

            entity.ToTable("IBS_DOCUMENT");

            entity.Property(e => e.Id)
                .HasPrecision(6)
                .HasDefaultValueSql("\"IBSDEV\".\"IBS_DOCUMENT_SEQ\".\"NEXTVAL\"")
                .HasColumnName("ID");
            entity.Property(e => e.Allowedfileextensions)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("ALLOWEDFILEEXTENSIONS");
            entity.Property(e => e.Documentcategory)
                .HasPrecision(6)
                .HasColumnName("DOCUMENTCATEGORY");
            entity.Property(e => e.Documentname)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("DOCUMENTNAME");
            entity.Property(e => e.Folderpath)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("FOLDERPATH");
            entity.Property(e => e.Isdownloadtemplate)
                .HasPrecision(2)
                .HasColumnName("ISDOWNLOADTEMPLATE");
            entity.Property(e => e.Ismandatory)
                .HasPrecision(2)
                .HasColumnName("ISMANDATORY");
            entity.Property(e => e.Isvideo)
                .HasPrecision(2)
                .HasColumnName("ISVIDEO");
            entity.Property(e => e.Isvisible)
                .HasPrecision(2)
                .HasColumnName("ISVISIBLE");
            entity.Property(e => e.Maxcontentlengthinkb)
                .HasPrecision(6)
                .HasColumnName("MAXCONTENTLENGTHINKB");
            entity.Property(e => e.Staticlink)
                .HasMaxLength(250)
                .HasColumnName("STATICLINK");
            entity.Property(e => e.Verifydsc)
                .HasPrecision(2)
                .HasColumnName("VERIFYDSC");
            entity.Property(e => e.Workflowstatusid)
                .HasPrecision(6)
                .HasColumnName("WORKFLOWSTATUSID");
        });

        modelBuilder.Entity<IbsDocumentcategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("IBS_DOCUMENTCATEGORY_PK");

            entity.ToTable("IBS_DOCUMENTCATEGORY");

            entity.Property(e => e.Id)
                .HasPrecision(6)
                .HasDefaultValueSql("\"IBSDEV\".\"IBS_DOCUMENTCATEGORY_SEQ\".\"NEXTVAL\"")
                .HasColumnName("ID");
            entity.Property(e => e.Categorylabel)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("CATEGORYLABEL");
            entity.Property(e => e.Categoryname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CATEGORYNAME");
            entity.Property(e => e.Groupid)
                .HasPrecision(6)
                .HasColumnName("GROUPID");
            entity.Property(e => e.Grouplabel)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("GROUPLABEL");
            entity.Property(e => e.Groupname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("GROUPNAME");
            entity.Property(e => e.Showlist)
                .HasPrecision(2)
                .HasColumnName("SHOWLIST");
        });

        modelBuilder.Entity<IbsSapIntegration>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("IBS_SAP_INTEGRATION");

            entity.Property(e => e.Amount01)
                .HasColumnType("NUMBER(13,2)")
                .HasColumnName("AMOUNT_01");
            entity.Property(e => e.Amount02)
                .HasColumnType("NUMBER(13,2)")
                .HasColumnName("AMOUNT_02");
            entity.Property(e => e.Amount03)
                .HasColumnType("NUMBER(13,2)")
                .HasColumnName("AMOUNT_03");
            entity.Property(e => e.Amount04)
                .HasColumnType("NUMBER(13,2)")
                .HasColumnName("AMOUNT_04");
            entity.Property(e => e.BusinessPlace01)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("BUSINESS_PLACE_01");
            entity.Property(e => e.BusinessPlace02)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("BUSINESS_PLACE_02");
            entity.Property(e => e.BusinessPlace03)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("BUSINESS_PLACE_03");
            entity.Property(e => e.CalculateTax01)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CALCULATE_TAX_01");
            entity.Property(e => e.CalculateTax02)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CALCULATE_TAX_02");
            entity.Property(e => e.CalculateTax03)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CALCULATE_TAX_03");
            entity.Property(e => e.Currency)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("CURRENCY");
            entity.Property(e => e.Customer01)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("CUSTOMER_01");
            entity.Property(e => e.Customer02)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("CUSTOMER_02");
            entity.Property(e => e.Customer03)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("CUSTOMER_03");
            entity.Property(e => e.DateOfCompletion01)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("DATE_OF_COMPLETION_01");
            entity.Property(e => e.DateOfCompletion02)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("DATE_OF_COMPLETION_02");
            entity.Property(e => e.DateOfCompletion03)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("DATE_OF_COMPLETION_03");
            entity.Property(e => e.DocumentDate)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("DOCUMENT_DATE");
            entity.Property(e => e.GlAccount)
                .HasPrecision(8)
                .HasColumnName("GL_ACCOUNT");
            entity.Property(e => e.GstPartner01)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("GST_PARTNER_01");
            entity.Property(e => e.GstPartner02)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("GST_PARTNER_02");
            entity.Property(e => e.GstPartner03)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("GST_PARTNER_03");
            entity.Property(e => e.GstTaxCode)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("GST_TAX_CODE");
            entity.Property(e => e.HsnSacCode)
                .HasPrecision(6)
                .HasColumnName("HSN_SAC_CODE");
            entity.Property(e => e.InvoiceType01)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("INVOICE_TYPE_01");
            entity.Property(e => e.InvoiceType02)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("INVOICE_TYPE_02");
            entity.Property(e => e.InvoiceType03)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("INVOICE_TYPE_03");
            entity.Property(e => e.PlaceOfSupply01)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("PLACE_OF_SUPPLY_01");
            entity.Property(e => e.PlaceOfSupply02)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("PLACE_OF_SUPPLY_02");
            entity.Property(e => e.PlaceOfSupply03)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("PLACE_OF_SUPPLY_03");
            entity.Property(e => e.PostingDate)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("POSTING_DATE");
            entity.Property(e => e.Reason)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("REASON");
            entity.Property(e => e.Reference01)
                .HasMaxLength(13)
                .IsUnicode(false)
                .HasColumnName("REFERENCE_01");
            entity.Property(e => e.Reference02)
                .HasMaxLength(13)
                .IsUnicode(false)
                .HasColumnName("REFERENCE_02");
            entity.Property(e => e.Reference03)
                .HasMaxLength(13)
                .IsUnicode(false)
                .HasColumnName("REFERENCE_03");
            entity.Property(e => e.SectionCode01)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("SECTION_CODE_01");
            entity.Property(e => e.SectionCode02)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("SECTION_CODE_02");
            entity.Property(e => e.SectionCode03)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("SECTION_CODE_03");
            entity.Property(e => e.Text01)
                .HasMaxLength(13)
                .IsUnicode(false)
                .HasColumnName("TEXT_01");
            entity.Property(e => e.Text02)
                .HasMaxLength(13)
                .IsUnicode(false)
                .HasColumnName("TEXT_02");
            entity.Property(e => e.Text03)
                .HasMaxLength(13)
                .IsUnicode(false)
                .HasColumnName("TEXT_03");
            entity.Property(e => e.Text04)
                .HasMaxLength(13)
                .IsUnicode(false)
                .HasColumnName("TEXT_04");
            entity.Property(e => e.Wbs)
                .HasPrecision(6)
                .HasColumnName("WBS");
        });

        modelBuilder.Entity<IbsToSapInvoice>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("IBS_TO_SAP_INVOICE");

            entity.Property(e => e.Amount01)
                .HasColumnType("NUMBER(13,2)")
                .HasColumnName("AMOUNT_01");
            entity.Property(e => e.Amount02)
                .HasColumnType("NUMBER(13,2)")
                .HasColumnName("AMOUNT_02");
            entity.Property(e => e.Amount03)
                .HasColumnType("NUMBER(13,2)")
                .HasColumnName("AMOUNT_03");
            entity.Property(e => e.Amount04)
                .HasColumnType("NUMBER(13,2)")
                .HasColumnName("AMOUNT_04");
            entity.Property(e => e.BusinessPlace01)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("BUSINESS_PLACE_01");
            entity.Property(e => e.BusinessPlace02)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("BUSINESS_PLACE_02");
            entity.Property(e => e.CalculateTax01)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CALCULATE_TAX_01");
            entity.Property(e => e.CalculateTax02)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CALCULATE_TAX_02");
            entity.Property(e => e.CalculateTax03)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CALCULATE_TAX_03");
            entity.Property(e => e.Currency)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CURRENCY");
            entity.Property(e => e.Customer01)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("CUSTOMER_01");
            entity.Property(e => e.Customer02)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("CUSTOMER_02");
            entity.Property(e => e.Customer03)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("CUSTOMER_03");
            entity.Property(e => e.DateOfCompletion01)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("DATE_OF_COMPLETION_01");
            entity.Property(e => e.DateOfCompletion02)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("DATE_OF_COMPLETION_02");
            entity.Property(e => e.DateOfCompletion03)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("DATE_OF_COMPLETION_03");
            entity.Property(e => e.DocumentDate)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("DOCUMENT_DATE");
            entity.Property(e => e.DocumentType01)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("DOCUMENT_TYPE_01");
            entity.Property(e => e.DocumentType02)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("DOCUMENT_TYPE_02");
            entity.Property(e => e.DocumenyType03)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("DOCUMENY_TYPE_03");
            entity.Property(e => e.GlAccount)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("GL_ACCOUNT");
            entity.Property(e => e.GstPartner01)
                .HasMaxLength(44)
                .IsUnicode(false)
                .HasColumnName("GST_PARTNER_01");
            entity.Property(e => e.GstPartner02)
                .HasMaxLength(44)
                .IsUnicode(false)
                .HasColumnName("GST_PARTNER_02");
            entity.Property(e => e.GstTaxCode)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("GST_TAX_CODE");
            entity.Property(e => e.HsnSacCode)
                .HasMaxLength(6)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("HSN_SAC_CODE");
            entity.Property(e => e.InvoiceType01)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("INVOICE_TYPE_01");
            entity.Property(e => e.InvoiceType02)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("INVOICE_TYPE_02");
            entity.Property(e => e.InvoiceType03)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("INVOICE_TYPE_03");
            entity.Property(e => e.PlaceOfSupply01)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("PLACE_OF_SUPPLY_01");
            entity.Property(e => e.PlaceOfSupply02)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("PLACE_OF_SUPPLY_02");
            entity.Property(e => e.PostingDate)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("POSTING_DATE");
            entity.Property(e => e.Reference01)
                .HasMaxLength(14)
                .IsUnicode(false)
                .HasColumnName("REFERENCE_01");
            entity.Property(e => e.Reference02)
                .HasMaxLength(14)
                .IsUnicode(false)
                .HasColumnName("REFERENCE_02");
            entity.Property(e => e.Reference03)
                .HasMaxLength(14)
                .IsUnicode(false)
                .HasColumnName("REFERENCE_03");
            entity.Property(e => e.SectionCode01)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("SECTION_CODE_01");
            entity.Property(e => e.SectionCode02)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("SECTION_CODE_02");
            entity.Property(e => e.Text01)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("TEXT_01");
            entity.Property(e => e.Text02)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("TEXT_02");
            entity.Property(e => e.Text03)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("TEXT_03");
            entity.Property(e => e.Wbs)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("WBS");
        });

        modelBuilder.Entity<IbsToSapInvoice01>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("IBS_TO_SAP_INVOICE_01");

            entity.Property(e => e.Amount01)
                .HasColumnType("NUMBER(13,2)")
                .HasColumnName("AMOUNT_01");
            entity.Property(e => e.Amount02)
                .HasColumnType("NUMBER(13,2)")
                .HasColumnName("AMOUNT_02");
            entity.Property(e => e.Amount03)
                .HasColumnType("NUMBER(13,2)")
                .HasColumnName("AMOUNT_03");
            entity.Property(e => e.Amount04)
                .HasColumnType("NUMBER(13,2)")
                .HasColumnName("AMOUNT_04");
            entity.Property(e => e.BusinessPlace01)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("BUSINESS_PLACE_01");
            entity.Property(e => e.BusinessPlace02)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("BUSINESS_PLACE_02");
            entity.Property(e => e.CalculateTax01)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CALCULATE_TAX_01");
            entity.Property(e => e.CalculateTax02)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CALCULATE_TAX_02");
            entity.Property(e => e.CalculateTax03)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CALCULATE_TAX_03");
            entity.Property(e => e.Currency)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CURRENCY");
            entity.Property(e => e.Customer01)
                .HasMaxLength(47)
                .IsUnicode(false)
                .HasColumnName("CUSTOMER_01");
            entity.Property(e => e.Customer02)
                .HasMaxLength(47)
                .IsUnicode(false)
                .HasColumnName("CUSTOMER_02");
            entity.Property(e => e.Customer03)
                .HasMaxLength(47)
                .IsUnicode(false)
                .HasColumnName("CUSTOMER_03");
            entity.Property(e => e.DateOfCompletion01)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("DATE_OF_COMPLETION_01");
            entity.Property(e => e.DateOfCompletion02)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("DATE_OF_COMPLETION_02");
            entity.Property(e => e.DateOfCompletion03)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("DATE_OF_COMPLETION_03");
            entity.Property(e => e.DocumentDate)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("DOCUMENT_DATE");
            entity.Property(e => e.DocumentType01)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("DOCUMENT_TYPE_01");
            entity.Property(e => e.DocumentType02)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("DOCUMENT_TYPE_02");
            entity.Property(e => e.DocumenyType03)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("DOCUMENY_TYPE_03");
            entity.Property(e => e.GlAccount)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("GL_ACCOUNT");
            entity.Property(e => e.GstPartner01)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("GST_PARTNER_01");
            entity.Property(e => e.GstPartner02)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("GST_PARTNER_02");
            entity.Property(e => e.GstTaxCode)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("GST_TAX_CODE");
            entity.Property(e => e.HsnSacCode)
                .HasMaxLength(6)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("HSN_SAC_CODE");
            entity.Property(e => e.InvoiceType01)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("INVOICE_TYPE_01");
            entity.Property(e => e.InvoiceType02)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("INVOICE_TYPE_02");
            entity.Property(e => e.InvoiceType03)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("INVOICE_TYPE_03");
            entity.Property(e => e.PlaceOfSupply01)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("PLACE_OF_SUPPLY_01");
            entity.Property(e => e.PlaceOfSupply02)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("PLACE_OF_SUPPLY_02");
            entity.Property(e => e.PostingDate)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("POSTING_DATE");
            entity.Property(e => e.Reference01)
                .HasMaxLength(14)
                .IsUnicode(false)
                .HasColumnName("REFERENCE_01");
            entity.Property(e => e.Reference02)
                .HasMaxLength(14)
                .IsUnicode(false)
                .HasColumnName("REFERENCE_02");
            entity.Property(e => e.Reference03)
                .HasMaxLength(14)
                .IsUnicode(false)
                .HasColumnName("REFERENCE_03");
            entity.Property(e => e.SectionCode01)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("SECTION_CODE_01");
            entity.Property(e => e.SectionCode02)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("SECTION_CODE_02");
            entity.Property(e => e.Text01)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("TEXT_01");
            entity.Property(e => e.Text02)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("TEXT_02");
            entity.Property(e => e.Text03)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("TEXT_03");
            entity.Property(e => e.Wbs)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("WBS");
        });

        modelBuilder.Entity<IbsToSapInvoiceCnote>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("IBS_TO_SAP_INVOICE_CNOTE");

            entity.Property(e => e.Amount01)
                .HasColumnType("NUMBER")
                .HasColumnName("AMOUNT_01");
            entity.Property(e => e.Amount02)
                .HasColumnType("NUMBER")
                .HasColumnName("AMOUNT_02");
            entity.Property(e => e.Amount03)
                .HasColumnType("NUMBER")
                .HasColumnName("AMOUNT_03");
            entity.Property(e => e.Amount04)
                .HasColumnType("NUMBER")
                .HasColumnName("AMOUNT_04");
            entity.Property(e => e.BusinessPlace01)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("BUSINESS_PLACE_01");
            entity.Property(e => e.BusinessPlace02)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("BUSINESS_PLACE_02");
            entity.Property(e => e.CalculateTax01)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CALCULATE_TAX_01");
            entity.Property(e => e.CalculateTax02)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CALCULATE_TAX_02");
            entity.Property(e => e.CalculateTax03)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CALCULATE_TAX_03");
            entity.Property(e => e.Currency)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CURRENCY");
            entity.Property(e => e.Customer01)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("CUSTOMER_01");
            entity.Property(e => e.Customer02)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("CUSTOMER_02");
            entity.Property(e => e.Customer03)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("CUSTOMER_03");
            entity.Property(e => e.DateOfCompletion01)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("DATE_OF_COMPLETION_01");
            entity.Property(e => e.DateOfCompletion02)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("DATE_OF_COMPLETION_02");
            entity.Property(e => e.DateOfCompletion03)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("DATE_OF_COMPLETION_03");
            entity.Property(e => e.DocumentDate)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("DOCUMENT_DATE");
            entity.Property(e => e.DocumentType01)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("DOCUMENT_TYPE_01");
            entity.Property(e => e.DocumentType02)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("DOCUMENT_TYPE_02");
            entity.Property(e => e.DocumenyType03)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("DOCUMENY_TYPE_03");
            entity.Property(e => e.GlAccount)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("GL_ACCOUNT");
            entity.Property(e => e.GstPartner01)
                .HasMaxLength(44)
                .IsUnicode(false)
                .HasColumnName("GST_PARTNER_01");
            entity.Property(e => e.GstPartner02)
                .HasMaxLength(44)
                .IsUnicode(false)
                .HasColumnName("GST_PARTNER_02");
            entity.Property(e => e.GstTaxCode)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("GST_TAX_CODE");
            entity.Property(e => e.HsnSacCode)
                .HasMaxLength(6)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("HSN_SAC_CODE");
            entity.Property(e => e.InvoiceType01)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("INVOICE_TYPE_01");
            entity.Property(e => e.InvoiceType02)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("INVOICE_TYPE_02");
            entity.Property(e => e.InvoiceType03)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("INVOICE_TYPE_03");
            entity.Property(e => e.PlaceOfSupply01)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("PLACE_OF_SUPPLY_01");
            entity.Property(e => e.PlaceOfSupply02)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("PLACE_OF_SUPPLY_02");
            entity.Property(e => e.PostingDate)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("POSTING_DATE");
            entity.Property(e => e.Reference01)
                .HasMaxLength(14)
                .IsUnicode(false)
                .HasColumnName("REFERENCE_01");
            entity.Property(e => e.Reference02)
                .HasMaxLength(14)
                .IsUnicode(false)
                .HasColumnName("REFERENCE_02");
            entity.Property(e => e.Reference03)
                .HasMaxLength(14)
                .IsUnicode(false)
                .HasColumnName("REFERENCE_03");
            entity.Property(e => e.SectionCode01)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("SECTION_CODE_01");
            entity.Property(e => e.SectionCode02)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("SECTION_CODE_02");
            entity.Property(e => e.Text01)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("TEXT_01");
            entity.Property(e => e.Text02)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("TEXT_02");
            entity.Property(e => e.Text03)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("TEXT_03");
            entity.Property(e => e.Wbs)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("WBS");
        });

        modelBuilder.Entity<IbsToSapInvoiceCnoteExt>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("IBS_TO_SAP_INVOICE_CNOTE_EXT");

            entity.Property(e => e.Amount01)
                .HasColumnType("NUMBER")
                .HasColumnName("AMOUNT_01");
            entity.Property(e => e.Amount02)
                .HasColumnType("NUMBER")
                .HasColumnName("AMOUNT_02");
            entity.Property(e => e.Amount03)
                .HasColumnType("NUMBER")
                .HasColumnName("AMOUNT_03");
            entity.Property(e => e.Amount04)
                .HasColumnType("NUMBER")
                .HasColumnName("AMOUNT_04");
            entity.Property(e => e.BusinessPlace01)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("BUSINESS_PLACE_01");
            entity.Property(e => e.BusinessPlace02)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("BUSINESS_PLACE_02");
            entity.Property(e => e.CalculateTax01)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CALCULATE_TAX_01");
            entity.Property(e => e.CalculateTax02)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CALCULATE_TAX_02");
            entity.Property(e => e.CalculateTax03)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CALCULATE_TAX_03");
            entity.Property(e => e.Currency)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CURRENCY");
            entity.Property(e => e.Customer01)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("CUSTOMER_01");
            entity.Property(e => e.Customer02)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("CUSTOMER_02");
            entity.Property(e => e.Customer03)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("CUSTOMER_03");
            entity.Property(e => e.DateOfCompletion01)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("DATE_OF_COMPLETION_01");
            entity.Property(e => e.DateOfCompletion02)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("DATE_OF_COMPLETION_02");
            entity.Property(e => e.DateOfCompletion03)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("DATE_OF_COMPLETION_03");
            entity.Property(e => e.DocumentDate)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("DOCUMENT_DATE");
            entity.Property(e => e.DocumentType01)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("DOCUMENT_TYPE_01");
            entity.Property(e => e.DocumentType02)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("DOCUMENT_TYPE_02");
            entity.Property(e => e.DocumenyType03)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("DOCUMENY_TYPE_03");
            entity.Property(e => e.GlAccount)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("GL_ACCOUNT");
            entity.Property(e => e.GstPartner01)
                .HasMaxLength(44)
                .IsUnicode(false)
                .HasColumnName("GST_PARTNER_01");
            entity.Property(e => e.GstPartner02)
                .HasMaxLength(44)
                .IsUnicode(false)
                .HasColumnName("GST_PARTNER_02");
            entity.Property(e => e.GstTaxCode)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("GST_TAX_CODE");
            entity.Property(e => e.HsnSacCode)
                .HasMaxLength(6)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("HSN_SAC_CODE");
            entity.Property(e => e.InvoiceType01)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("INVOICE_TYPE_01");
            entity.Property(e => e.InvoiceType02)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("INVOICE_TYPE_02");
            entity.Property(e => e.InvoiceType03)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("INVOICE_TYPE_03");
            entity.Property(e => e.PlaceOfSupply01)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("PLACE_OF_SUPPLY_01");
            entity.Property(e => e.PlaceOfSupply02)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("PLACE_OF_SUPPLY_02");
            entity.Property(e => e.PostingDate)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("POSTING_DATE");
            entity.Property(e => e.Reference01)
                .HasMaxLength(14)
                .IsUnicode(false)
                .HasColumnName("REFERENCE_01");
            entity.Property(e => e.Reference02)
                .HasMaxLength(14)
                .IsUnicode(false)
                .HasColumnName("REFERENCE_02");
            entity.Property(e => e.Reference03)
                .HasMaxLength(14)
                .IsUnicode(false)
                .HasColumnName("REFERENCE_03");
            entity.Property(e => e.SectionCode01)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("SECTION_CODE_01");
            entity.Property(e => e.SectionCode02)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("SECTION_CODE_02");
            entity.Property(e => e.Text01)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("TEXT_01");
            entity.Property(e => e.Text02)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("TEXT_02");
            entity.Property(e => e.Text03)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("TEXT_03");
            entity.Property(e => e.Wbs)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("WBS");
        });

        modelBuilder.Entity<IbsToSapInvoiceExt>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("IBS_TO_SAP_INVOICE_EXT");

            entity.Property(e => e.Amount01)
                .HasColumnType("NUMBER(13,2)")
                .HasColumnName("AMOUNT_01");
            entity.Property(e => e.Amount02)
                .HasColumnType("NUMBER(13,2)")
                .HasColumnName("AMOUNT_02");
            entity.Property(e => e.Amount03)
                .HasColumnType("NUMBER(13,2)")
                .HasColumnName("AMOUNT_03");
            entity.Property(e => e.Amount04)
                .HasColumnType("NUMBER(13,2)")
                .HasColumnName("AMOUNT_04");
            entity.Property(e => e.BusinessPlace01)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("BUSINESS_PLACE_01");
            entity.Property(e => e.BusinessPlace02)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("BUSINESS_PLACE_02");
            entity.Property(e => e.CalculateTax01)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CALCULATE_TAX_01");
            entity.Property(e => e.CalculateTax02)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CALCULATE_TAX_02");
            entity.Property(e => e.CalculateTax03)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CALCULATE_TAX_03");
            entity.Property(e => e.Currency)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CURRENCY");
            entity.Property(e => e.Customer01)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("CUSTOMER_01");
            entity.Property(e => e.Customer02)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("CUSTOMER_02");
            entity.Property(e => e.Customer03)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("CUSTOMER_03");
            entity.Property(e => e.DateOfCompletion01)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("DATE_OF_COMPLETION_01");
            entity.Property(e => e.DateOfCompletion02)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("DATE_OF_COMPLETION_02");
            entity.Property(e => e.DateOfCompletion03)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("DATE_OF_COMPLETION_03");
            entity.Property(e => e.DocumentDate)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("DOCUMENT_DATE");
            entity.Property(e => e.DocumentType01)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("DOCUMENT_TYPE_01");
            entity.Property(e => e.DocumentType02)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("DOCUMENT_TYPE_02");
            entity.Property(e => e.DocumenyType03)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("DOCUMENY_TYPE_03");
            entity.Property(e => e.GlAccount)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("GL_ACCOUNT");
            entity.Property(e => e.GstPartner01)
                .HasMaxLength(44)
                .IsUnicode(false)
                .HasColumnName("GST_PARTNER_01");
            entity.Property(e => e.GstPartner02)
                .HasMaxLength(44)
                .IsUnicode(false)
                .HasColumnName("GST_PARTNER_02");
            entity.Property(e => e.GstTaxCode)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("GST_TAX_CODE");
            entity.Property(e => e.HsnSacCode)
                .HasMaxLength(6)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("HSN_SAC_CODE");
            entity.Property(e => e.InvoiceType01)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("INVOICE_TYPE_01");
            entity.Property(e => e.InvoiceType02)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("INVOICE_TYPE_02");
            entity.Property(e => e.InvoiceType03)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("INVOICE_TYPE_03");
            entity.Property(e => e.PlaceOfSupply01)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("PLACE_OF_SUPPLY_01");
            entity.Property(e => e.PlaceOfSupply02)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("PLACE_OF_SUPPLY_02");
            entity.Property(e => e.PostingDate)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("POSTING_DATE");
            entity.Property(e => e.Reference01)
                .HasMaxLength(14)
                .IsUnicode(false)
                .HasColumnName("REFERENCE_01");
            entity.Property(e => e.Reference02)
                .HasMaxLength(14)
                .IsUnicode(false)
                .HasColumnName("REFERENCE_02");
            entity.Property(e => e.Reference03)
                .HasMaxLength(14)
                .IsUnicode(false)
                .HasColumnName("REFERENCE_03");
            entity.Property(e => e.SectionCode01)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("SECTION_CODE_01");
            entity.Property(e => e.SectionCode02)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("SECTION_CODE_02");
            entity.Property(e => e.Text01)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("TEXT_01");
            entity.Property(e => e.Text02)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("TEXT_02");
            entity.Property(e => e.Text03)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("TEXT_03");
            entity.Property(e => e.Wbs)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("WBS");
        });

        modelBuilder.Entity<IbsToSapInvoiceExt01>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("IBS_TO_SAP_INVOICE_EXT_01");

            entity.Property(e => e.Amount01)
                .HasColumnType("NUMBER(13,2)")
                .HasColumnName("AMOUNT_01");
            entity.Property(e => e.Amount02)
                .HasColumnType("NUMBER(13,2)")
                .HasColumnName("AMOUNT_02");
            entity.Property(e => e.Amount03)
                .HasColumnType("NUMBER(13,2)")
                .HasColumnName("AMOUNT_03");
            entity.Property(e => e.Amount04)
                .HasColumnType("NUMBER(13,2)")
                .HasColumnName("AMOUNT_04");
            entity.Property(e => e.BusinessPlace01)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("BUSINESS_PLACE_01");
            entity.Property(e => e.BusinessPlace02)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("BUSINESS_PLACE_02");
            entity.Property(e => e.CalculateTax01)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CALCULATE_TAX_01");
            entity.Property(e => e.CalculateTax02)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CALCULATE_TAX_02");
            entity.Property(e => e.CalculateTax03)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CALCULATE_TAX_03");
            entity.Property(e => e.Currency)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CURRENCY");
            entity.Property(e => e.Customer01)
                .HasMaxLength(47)
                .IsUnicode(false)
                .HasColumnName("CUSTOMER_01");
            entity.Property(e => e.Customer02)
                .HasMaxLength(47)
                .IsUnicode(false)
                .HasColumnName("CUSTOMER_02");
            entity.Property(e => e.Customer03)
                .HasMaxLength(47)
                .IsUnicode(false)
                .HasColumnName("CUSTOMER_03");
            entity.Property(e => e.DateOfCompletion01)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("DATE_OF_COMPLETION_01");
            entity.Property(e => e.DateOfCompletion02)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("DATE_OF_COMPLETION_02");
            entity.Property(e => e.DateOfCompletion03)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("DATE_OF_COMPLETION_03");
            entity.Property(e => e.DocumentDate)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("DOCUMENT_DATE");
            entity.Property(e => e.DocumentType01)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("DOCUMENT_TYPE_01");
            entity.Property(e => e.DocumentType02)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("DOCUMENT_TYPE_02");
            entity.Property(e => e.DocumenyType03)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("DOCUMENY_TYPE_03");
            entity.Property(e => e.GlAccount)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("GL_ACCOUNT");
            entity.Property(e => e.GstPartner01)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("GST_PARTNER_01");
            entity.Property(e => e.GstPartner02)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("GST_PARTNER_02");
            entity.Property(e => e.GstTaxCode)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("GST_TAX_CODE");
            entity.Property(e => e.HsnSacCode)
                .HasMaxLength(6)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("HSN_SAC_CODE");
            entity.Property(e => e.InvoiceType01)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("INVOICE_TYPE_01");
            entity.Property(e => e.InvoiceType02)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("INVOICE_TYPE_02");
            entity.Property(e => e.InvoiceType03)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("INVOICE_TYPE_03");
            entity.Property(e => e.PlaceOfSupply01)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("PLACE_OF_SUPPLY_01");
            entity.Property(e => e.PlaceOfSupply02)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("PLACE_OF_SUPPLY_02");
            entity.Property(e => e.PostingDate)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("POSTING_DATE");
            entity.Property(e => e.Reference01)
                .HasMaxLength(14)
                .IsUnicode(false)
                .HasColumnName("REFERENCE_01");
            entity.Property(e => e.Reference02)
                .HasMaxLength(14)
                .IsUnicode(false)
                .HasColumnName("REFERENCE_02");
            entity.Property(e => e.Reference03)
                .HasMaxLength(14)
                .IsUnicode(false)
                .HasColumnName("REFERENCE_03");
            entity.Property(e => e.SectionCode01)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("SECTION_CODE_01");
            entity.Property(e => e.SectionCode02)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("SECTION_CODE_02");
            entity.Property(e => e.Text01)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("TEXT_01");
            entity.Property(e => e.Text02)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("TEXT_02");
            entity.Property(e => e.Text03)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("TEXT_03");
            entity.Property(e => e.Wbs)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("WBS");
        });

        modelBuilder.Entity<IbslabToSapInvoice>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("IBSLAB_TO_SAP_INVOICE");

            entity.Property(e => e.Amount01)
                .HasColumnType("NUMBER")
                .HasColumnName("AMOUNT_01");
            entity.Property(e => e.Amount02)
                .HasColumnType("NUMBER")
                .HasColumnName("AMOUNT_02");
            entity.Property(e => e.Amount03)
                .HasColumnType("NUMBER")
                .HasColumnName("AMOUNT_03");
            entity.Property(e => e.Amount04)
                .HasColumnType("NUMBER")
                .HasColumnName("AMOUNT_04");
            entity.Property(e => e.BusinessPlace01)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("BUSINESS_PLACE_01");
            entity.Property(e => e.BusinessPlace02)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("BUSINESS_PLACE_02");
            entity.Property(e => e.CalculateTax01)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CALCULATE_TAX_01");
            entity.Property(e => e.CalculateTax02)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CALCULATE_TAX_02");
            entity.Property(e => e.CalculateTax03)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CALCULATE_TAX_03");
            entity.Property(e => e.Currency)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CURRENCY");
            entity.Property(e => e.Customer01)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("CUSTOMER_01");
            entity.Property(e => e.Customer02)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("CUSTOMER_02");
            entity.Property(e => e.Customer03)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("CUSTOMER_03");
            entity.Property(e => e.DateOfCompletion01)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("DATE_OF_COMPLETION_01");
            entity.Property(e => e.DateOfCompletion02)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("DATE_OF_COMPLETION_02");
            entity.Property(e => e.DateOfCompletion03)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("DATE_OF_COMPLETION_03");
            entity.Property(e => e.DocumentDate)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("DOCUMENT_DATE");
            entity.Property(e => e.DocumentType01)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("DOCUMENT_TYPE_01");
            entity.Property(e => e.DocumentType02)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("DOCUMENT_TYPE_02");
            entity.Property(e => e.DocumenyType03)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("DOCUMENY_TYPE_03");
            entity.Property(e => e.GlAccount)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("GL_ACCOUNT");
            entity.Property(e => e.GstPartner01)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("GST_PARTNER_01");
            entity.Property(e => e.GstPartner02)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("GST_PARTNER_02");
            entity.Property(e => e.GstTaxCode)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("GST_TAX_CODE");
            entity.Property(e => e.HsnSacCode)
                .HasMaxLength(6)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("HSN_SAC_CODE");
            entity.Property(e => e.InvoiceType01)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("INVOICE_TYPE_01");
            entity.Property(e => e.InvoiceType02)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("INVOICE_TYPE_02");
            entity.Property(e => e.InvoiceType03)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("INVOICE_TYPE_03");
            entity.Property(e => e.PlaceOfSupply01)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("PLACE_OF_SUPPLY_01");
            entity.Property(e => e.PlaceOfSupply02)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("PLACE_OF_SUPPLY_02");
            entity.Property(e => e.PostingDate)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("POSTING_DATE");
            entity.Property(e => e.Reference01)
                .HasMaxLength(14)
                .IsUnicode(false)
                .HasColumnName("REFERENCE_01");
            entity.Property(e => e.Reference02)
                .HasMaxLength(14)
                .IsUnicode(false)
                .HasColumnName("REFERENCE_02");
            entity.Property(e => e.Reference03)
                .HasMaxLength(14)
                .IsUnicode(false)
                .HasColumnName("REFERENCE_03");
            entity.Property(e => e.SectionCode01)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("SECTION_CODE_01");
            entity.Property(e => e.SectionCode02)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("SECTION_CODE_02");
            entity.Property(e => e.Text01)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("TEXT_01");
            entity.Property(e => e.Text02)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("TEXT_02");
            entity.Property(e => e.Text03)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("TEXT_03");
            entity.Property(e => e.Wbs)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("WBS");
        });

        modelBuilder.Entity<IbslabToSapInvoiceExt>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("IBSLAB_TO_SAP_INVOICE_EXT");

            entity.Property(e => e.Amount01)
                .HasColumnType("NUMBER")
                .HasColumnName("AMOUNT_01");
            entity.Property(e => e.Amount02)
                .HasColumnType("NUMBER")
                .HasColumnName("AMOUNT_02");
            entity.Property(e => e.Amount03)
                .HasColumnType("NUMBER")
                .HasColumnName("AMOUNT_03");
            entity.Property(e => e.Amount04)
                .HasColumnType("NUMBER")
                .HasColumnName("AMOUNT_04");
            entity.Property(e => e.BusinessPlace01)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("BUSINESS_PLACE_01");
            entity.Property(e => e.BusinessPlace02)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("BUSINESS_PLACE_02");
            entity.Property(e => e.CalculateTax01)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CALCULATE_TAX_01");
            entity.Property(e => e.CalculateTax02)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CALCULATE_TAX_02");
            entity.Property(e => e.CalculateTax03)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CALCULATE_TAX_03");
            entity.Property(e => e.Currency)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CURRENCY");
            entity.Property(e => e.Customer01)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("CUSTOMER_01");
            entity.Property(e => e.Customer02)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("CUSTOMER_02");
            entity.Property(e => e.Customer03)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("CUSTOMER_03");
            entity.Property(e => e.DateOfCompletion01)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("DATE_OF_COMPLETION_01");
            entity.Property(e => e.DateOfCompletion02)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("DATE_OF_COMPLETION_02");
            entity.Property(e => e.DateOfCompletion03)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("DATE_OF_COMPLETION_03");
            entity.Property(e => e.DocumentDate)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("DOCUMENT_DATE");
            entity.Property(e => e.DocumentType01)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("DOCUMENT_TYPE_01");
            entity.Property(e => e.DocumentType02)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("DOCUMENT_TYPE_02");
            entity.Property(e => e.DocumenyType03)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("DOCUMENY_TYPE_03");
            entity.Property(e => e.GlAccount)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("GL_ACCOUNT");
            entity.Property(e => e.GstPartner01)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("GST_PARTNER_01");
            entity.Property(e => e.GstPartner02)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("GST_PARTNER_02");
            entity.Property(e => e.GstTaxCode)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("GST_TAX_CODE");
            entity.Property(e => e.HsnSacCode)
                .HasMaxLength(6)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("HSN_SAC_CODE");
            entity.Property(e => e.InvoiceType01)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("INVOICE_TYPE_01");
            entity.Property(e => e.InvoiceType02)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("INVOICE_TYPE_02");
            entity.Property(e => e.InvoiceType03)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("INVOICE_TYPE_03");
            entity.Property(e => e.PlaceOfSupply01)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("PLACE_OF_SUPPLY_01");
            entity.Property(e => e.PlaceOfSupply02)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("PLACE_OF_SUPPLY_02");
            entity.Property(e => e.PostingDate)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("POSTING_DATE");
            entity.Property(e => e.Reference01)
                .HasMaxLength(14)
                .IsUnicode(false)
                .HasColumnName("REFERENCE_01");
            entity.Property(e => e.Reference02)
                .HasMaxLength(14)
                .IsUnicode(false)
                .HasColumnName("REFERENCE_02");
            entity.Property(e => e.Reference03)
                .HasMaxLength(14)
                .IsUnicode(false)
                .HasColumnName("REFERENCE_03");
            entity.Property(e => e.SectionCode01)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("SECTION_CODE_01");
            entity.Property(e => e.SectionCode02)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("SECTION_CODE_02");
            entity.Property(e => e.Text01)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("TEXT_01");
            entity.Property(e => e.Text02)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("TEXT_02");
            entity.Property(e => e.Text03)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("TEXT_03");
            entity.Property(e => e.Wbs)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("WBS");
        });

        modelBuilder.Entity<IcIntermediate>(entity =>
        {
            entity.HasKey(e => new { e.CaseNo, e.CallRecvDt, e.CallSno, e.ConsigneeCd, e.BkNo, e.SetNo, e.ItemSrnoPo });

            entity.ToTable("IC_INTERMEDIATE");

            entity.Property(e => e.CaseNo)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CASE_NO");
            entity.Property(e => e.CallRecvDt)
                .HasColumnType("DATE")
                .HasColumnName("CALL_RECV_DT");
            entity.Property(e => e.CallSno)
                .HasPrecision(5)
                .HasColumnName("CALL_SNO");
            entity.Property(e => e.ConsigneeCd)
                .HasPrecision(8)
                .HasColumnName("CONSIGNEE_CD");
            entity.Property(e => e.BkNo)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("BK_NO");
            entity.Property(e => e.SetNo)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("SET_NO");
            entity.Property(e => e.ItemSrnoPo)
                .HasPrecision(4)
                .HasColumnName("ITEM_SRNO_PO");
            entity.Property(e => e.Amendment1)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("AMENDMENT_1");
            entity.Property(e => e.Amendment2)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("AMENDMENT_2");
            entity.Property(e => e.Amendment3)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("AMENDMENT_3");
            entity.Property(e => e.Amendment4)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("AMENDMENT_4");
            entity.Property(e => e.BpoDtl)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("BPO_DTL");
            entity.Property(e => e.ConsgnCallStatus)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CONSGN_CALL_STATUS");
            entity.Property(e => e.ConsigneeDesg)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CONSIGNEE_DESG");
            entity.Property(e => e.ConsigneeDtl)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("CONSIGNEE_DTL");
            entity.Property(e => e.CumQtyPrevOffered)
                .HasColumnType("NUMBER(12,4)")
                .HasColumnName("CUM_QTY_PREV_OFFERED");
            entity.Property(e => e.CumQtyPrevPassed)
                .HasColumnType("NUMBER(12,4)")
                .HasColumnName("CUM_QTY_PREV_PASSED");
            entity.Property(e => e.Datetime)
                .HasPrecision(6)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("DATETIME");
            entity.Property(e => e.DispatchPackingNo)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("DISPATCH_PACKING_NO");
            entity.Property(e => e.File1)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("FILE_1");
            entity.Property(e => e.File10)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("FILE_10");
            entity.Property(e => e.File2)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("FILE_2");
            entity.Property(e => e.File3)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("FILE_3");
            entity.Property(e => e.File4)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("FILE_4");
            entity.Property(e => e.File5)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("FILE_5");
            entity.Property(e => e.File6)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("FILE_6");
            entity.Property(e => e.File7)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("FILE_7");
            entity.Property(e => e.File8)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("FILE_8");
            entity.Property(e => e.File9)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("FILE_9");
            entity.Property(e => e.GovBillAuth)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("GOV_BILL_AUTH");
            entity.Property(e => e.Hologram)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .HasColumnName("HOLOGRAM");
            entity.Property(e => e.IeCd)
                .HasPrecision(4)
                .HasColumnName("IE_CD");
            entity.Property(e => e.IeStamp)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("IE_STAMP");
            entity.Property(e => e.IeStamp2)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("IE_STAMP2");
            entity.Property(e => e.IeStampCd)
                .HasPrecision(3)
                .HasColumnName("IE_STAMP_CD");
            entity.Property(e => e.IeStampImage)
                .HasColumnType("BLOB")
                .HasColumnName("IE_STAMP_IMAGE");
            entity.Property(e => e.IeStampImage1)
                .HasColumnType("BLOB")
                .HasColumnName("IE_STAMP_IMAGE1");
            entity.Property(e => e.IeStampsDetail)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("IE_STAMPS_DETAIL");
            entity.Property(e => e.IeStampsDetail2)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("IE_STAMPS_DETAIL2");
            entity.Property(e => e.InvoiceNo)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("INVOICE_NO");
            entity.Property(e => e.ItemDescPo)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .HasColumnName("ITEM_DESC_PO");
            entity.Property(e => e.ItemRemark)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("ITEM_REMARK");
            entity.Property(e => e.LabTstRectDt)
                .HasColumnType("DATE")
                .HasColumnName("LAB_TST_RECT_DT");
            entity.Property(e => e.ManType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("MAN_TYPE");
            entity.Property(e => e.NameOfIe)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("NAME_OF_IE");
            entity.Property(e => e.NumVisits)
                .HasPrecision(9)
                .HasColumnName("NUM_VISITS");
            entity.Property(e => e.OffInstNoDtl)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("OFF_INST_NO_DTL");
            entity.Property(e => e.PassedInstNo)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("PASSED_INST_NO");
            entity.Property(e => e.PoNo)
                .HasMaxLength(75)
                .IsUnicode(false)
                .HasColumnName("PO_NO");
            entity.Property(e => e.PurAutDtl)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("PUR_AUT_DTL");
            entity.Property(e => e.PurDtl)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("PUR_DTL");
            entity.Property(e => e.QtyDue)
                .HasColumnType("NUMBER(12,4)")
                .HasColumnName("QTY_DUE");
            entity.Property(e => e.QtyOrdered)
                .HasColumnType("NUMBER(12,4)")
                .HasColumnName("QTY_ORDERED");
            entity.Property(e => e.QtyPassed)
                .HasColumnType("NUMBER(12,4)")
                .HasColumnName("QTY_PASSED");
            entity.Property(e => e.QtyRejected)
                .HasColumnType("NUMBER(12,4)")
                .HasColumnName("QTY_REJECTED");
            entity.Property(e => e.QtyToInsp)
                .HasColumnType("NUMBER(12,4)")
                .HasColumnName("QTY_TO_INSP");
            entity.Property(e => e.ReasonOfRejection)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .HasColumnName("REASON_OF_REJECTION");
            entity.Property(e => e.Remark)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .HasColumnName("REMARK");
            entity.Property(e => e.UnitDtl)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("UNIT_DTL");
            entity.Property(e => e.UserId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("USER_ID");
            entity.Property(e => e.VisitsDates)
                .HasMaxLength(800)
                .IsUnicode(false)
                .HasColumnName("VISITS_DATES");
        });

        modelBuilder.Entity<IcPoAmendment>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("IC_PO_AMENDMENT");

            entity.Property(e => e.AmendmentDetail)
                .IsUnicode(false)
                .HasColumnName("AMENDMENT_DETAIL");
            entity.Property(e => e.CaseNo)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CASE_NO");
            entity.Property(e => e.PoNo)
                .HasMaxLength(75)
                .IsUnicode(false)
                .HasColumnName("PO_NO");
        });

        modelBuilder.Entity<IcVisit>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("IC_VISITS");

            entity.Property(e => e.CallRecvDt)
                .HasColumnType("DATE")
                .HasColumnName("CALL_RECV_DT");
            entity.Property(e => e.CallSno)
                .HasPrecision(5)
                .HasColumnName("CALL_SNO");
            entity.Property(e => e.CaseNo)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CASE_NO");
            entity.Property(e => e.VisitsDates)
                .HasMaxLength(800)
                .IsUnicode(false)
                .HasColumnName("VISITS_DATES");
        });

        modelBuilder.Entity<IeCallLocationEntryexit>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("IE_CALL_LOCATION_ENTRYEXIT");

            entity.Property(e => e.CallRecvDt)
                .HasColumnType("DATE")
                .HasColumnName("CALL_RECV_DT");
            entity.Property(e => e.CallSno)
                .HasPrecision(5)
                .HasColumnName("CALL_SNO");
            entity.Property(e => e.CaseNo)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CASE_NO");
            entity.Property(e => e.EntryDatetime)
                .HasColumnType("DATE")
                .HasColumnName("ENTRY_DATETIME");
            entity.Property(e => e.ExitDatetime)
                .HasColumnType("DATE")
                .HasColumnName("EXIT_DATETIME");
            entity.Property(e => e.IeCd)
                .HasPrecision(3)
                .HasColumnName("IE_CD");

            entity.HasOne(d => d.Ca).WithMany()
                .HasForeignKey(d => new { d.CaseNo, d.CallRecvDt, d.CallSno })
                .HasConstraintName("FK_IE_CALL_LOCATION");
        });

        modelBuilder.Entity<IePlanCallTime>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("IE_PLAN_CALL_TIME");

            entity.Property(e => e.CallEndtime)
                .HasColumnType("DATE")
                .HasColumnName("CALL_ENDTIME");
            entity.Property(e => e.CallRecvDt)
                .HasColumnType("DATE")
                .HasColumnName("CALL_RECV_DT");
            entity.Property(e => e.CallSno)
                .HasPrecision(5)
                .HasColumnName("CALL_SNO");
            entity.Property(e => e.CallStarttime)
                .HasColumnType("DATE")
                .HasColumnName("CALL_STARTTIME");
            entity.Property(e => e.CaseNo)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CASE_NO");
            entity.Property(e => e.IeCd)
                .HasPrecision(3)
                .HasColumnName("IE_CD");

            entity.HasOne(d => d.Ca).WithMany()
                .HasForeignKey(d => new { d.CaseNo, d.CallRecvDt, d.CallSno })
                .HasConstraintName("FK_IE_PLAN_CALL");
        });

        modelBuilder.Entity<IeStamp>(entity =>
        {
            entity.HasKey(e => e.IeStampCd).HasName("PK_IE_STAMP_CD");

            entity.ToTable("IE_STAMPS");

            entity.Property(e => e.IeStampCd)
                .HasPrecision(3)
                .HasColumnName("IE_STAMP_CD");
            entity.Property(e => e.IeStampsDetail)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("IE_STAMPS_DETAIL");
        });

        modelBuilder.Entity<ImmsRitesPoDetail>(entity =>
        {
            entity.HasKey(e => e.Oid).HasName("IMMS_RITES_PO_DETAIL_PK");

            entity.ToTable("IMMS_RITES_PO_DETAIL");

            entity.Property(e => e.Oid)
                .HasColumnType("NUMBER")
                .HasColumnName("OID");
            entity.Property(e => e.Allocation)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("ALLOCATION");
            entity.Property(e => e.BasicValue)
                .HasColumnType("NUMBER(15,3)")
                .HasColumnName("BASIC_VALUE");
            entity.Property(e => e.ConsigneeCd)
                .HasPrecision(8)
                .HasColumnName("CONSIGNEE_CD");
            entity.Property(e => e.Datetime)
                .HasColumnType("DATE")
                .HasColumnName("DATETIME");
            entity.Property(e => e.DelvDt)
                .HasColumnType("DATE")
                .HasColumnName("DELV_DT");
            entity.Property(e => e.Discount)
                .HasColumnType("NUMBER(15,4)")
                .HasColumnName("DISCOUNT");
            entity.Property(e => e.DiscountPer)
                .HasColumnType("NUMBER(15,4)")
                .HasColumnName("DISCOUNT_PER");
            entity.Property(e => e.DiscountType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("DISCOUNT_TYPE");
            entity.Property(e => e.Excise)
                .HasColumnType("NUMBER(15,4)")
                .HasColumnName("EXCISE");
            entity.Property(e => e.ExcisePer)
                .HasColumnType("NUMBER(15,4)")
                .HasColumnName("EXCISE_PER");
            entity.Property(e => e.ExciseType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("EXCISE_TYPE");
            entity.Property(e => e.ExtDelvDt)
                .HasColumnType("DATE")
                .HasColumnName("EXT_DELV_DT");
            entity.Property(e => e.ImmsConsigneeCd)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("IMMS_CONSIGNEE_CD");
            entity.Property(e => e.ImmsConsigneeDetail)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("IMMS_CONSIGNEE_DETAIL");
            entity.Property(e => e.ImmsConsigneeName)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("IMMS_CONSIGNEE_NAME");
            entity.Property(e => e.ImmsPokey)
                .HasPrecision(10)
                .HasColumnName("IMMS_POKEY");
            entity.Property(e => e.ImmsRlyCd)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("IMMS_RLY_CD");
            entity.Property(e => e.ImmsUomCd)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("IMMS_UOM_CD");
            entity.Property(e => e.ImmsUomDesc)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("IMMS_UOM_DESC");
            entity.Property(e => e.ItemCd)
                .HasMaxLength(7)
                .IsUnicode(false)
                .HasColumnName("ITEM_CD");
            entity.Property(e => e.ItemDesc)
                .IsUnicode(false)
                .HasColumnName("ITEM_DESC");
            entity.Property(e => e.ItemSrno)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("ITEM_SRNO");
            entity.Property(e => e.OtChargePer)
                .HasColumnType("NUMBER(15,4)")
                .HasColumnName("OT_CHARGE_PER");
            entity.Property(e => e.OtChargeType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("OT_CHARGE_TYPE");
            entity.Property(e => e.OtCharges)
                .HasColumnType("NUMBER(15,4)")
                .HasColumnName("OT_CHARGES");
            entity.Property(e => e.PlNo)
                .HasMaxLength(14)
                .IsUnicode(false)
                .HasColumnName("PL_NO");
            entity.Property(e => e.Qty)
                .HasColumnType("NUMBER(14,3)")
                .HasColumnName("QTY");
            entity.Property(e => e.Rate)
                .HasColumnType("NUMBER(15,3)")
                .HasColumnName("RATE");
            entity.Property(e => e.SalesTax)
                .HasColumnType("NUMBER(15,4)")
                .HasColumnName("SALES_TAX");
            entity.Property(e => e.SalesTaxPer)
                .HasColumnType("NUMBER(15,4)")
                .HasColumnName("SALES_TAX_PER");
            entity.Property(e => e.UomCd)
                .HasPrecision(3)
                .HasColumnName("UOM_CD");
            entity.Property(e => e.UserId)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("USER_ID");
            entity.Property(e => e.Value)
                .HasColumnType("NUMBER(14,2)")
                .HasColumnName("VALUE");
        });

        modelBuilder.Entity<ImmsRitesPoHdr>(entity =>
        {
            entity.HasKey(e => e.PoId).HasName("IMMS_RITES_PO_HDR_NEW_PK");

            entity.ToTable("IMMS_RITES_PO_HDR");

            entity.Property(e => e.PoId)
                .HasColumnType("NUMBER")
                .HasColumnName("PO_ID");
            entity.Property(e => e.AckFlag)
                .HasPrecision(2)
                .HasColumnName("ACK_FLAG");
            entity.Property(e => e.BpoCd)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("BPO_CD");
            entity.Property(e => e.Datetime)
                .HasColumnType("DATE")
                .HasColumnName("DATETIME");
            entity.Property(e => e.ImmsBpoCd)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("IMMS_BPO_CD");
            entity.Property(e => e.ImmsBpoDetail)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("IMMS_BPO_DETAIL");
            entity.Property(e => e.ImmsBpoName)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("IMMS_BPO_NAME");
            entity.Property(e => e.ImmsPoiDetail)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("IMMS_POI_DETAIL");
            entity.Property(e => e.ImmsPoiName)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("IMMS_POI_NAME");
            entity.Property(e => e.ImmsPokey)
                .HasPrecision(10)
                .HasColumnName("IMMS_POKEY");
            entity.Property(e => e.ImmsPurchaserCd)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("IMMS_PURCHASER_CD");
            entity.Property(e => e.ImmsPurchaserDetail)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("IMMS_PURCHASER_DETAIL");
            entity.Property(e => e.ImmsRlyCd)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("IMMS_RLY_CD");
            entity.Property(e => e.ImmsRlyShortname)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("IMMS_RLY_SHORTNAME");
            entity.Property(e => e.ImmsVendorCd)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("IMMS_VENDOR_CD");
            entity.Property(e => e.ImmsVendorCity)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("IMMS_VENDOR_CITY");
            entity.Property(e => e.ImmsVendorDetail)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("IMMS_VENDOR_DETAIL");
            entity.Property(e => e.ImmsVendorName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("IMMS_VENDOR_NAME");
            entity.Property(e => e.InspectingAgency)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("INSPECTING_AGENCY");
            entity.Property(e => e.L5noPo)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("L5NO_PO");
            entity.Property(e => e.PoDt)
                .HasColumnType("DATE")
                .HasColumnName("PO_DT");
            entity.Property(e => e.PoNo)
                .HasMaxLength(14)
                .IsUnicode(false)
                .HasColumnName("PO_NO");
            entity.Property(e => e.PoOrLetter)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("PO_OR_LETTER");
            entity.Property(e => e.PoiCd)
                .HasPrecision(6)
                .HasColumnName("POI_CD");
            entity.Property(e => e.PurchaserCd)
                .HasPrecision(8)
                .HasColumnName("PURCHASER_CD");
            entity.Property(e => e.RecvDate)
                .HasColumnType("DATE")
                .HasColumnName("RECV_DATE");
            entity.Property(e => e.RegionCode)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("REGION_CODE");
            entity.Property(e => e.Remarks)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("REMARKS");
            entity.Property(e => e.RitesCaseNo)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("RITES_CASE_NO");
            entity.Property(e => e.RlyCd)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("RLY_CD");
            entity.Property(e => e.RlyNonrly)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("RLY_NONRLY");
            entity.Property(e => e.StockNonstock)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("STOCK_NONSTOCK");
            entity.Property(e => e.UserId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("USER_ID");
            entity.Property(e => e.Valid)
                .HasPrecision(1)
                .HasColumnName("VALID");
            entity.Property(e => e.VendCd)
                .HasPrecision(6)
                .HasColumnName("VEND_CD");
        });

        modelBuilder.Entity<ImmsRitesPocaDtl>(entity =>
        {
            entity.HasKey(e => new { e.Rly, e.Cakey, e.Slno }).HasName("IMMS_RITES_POCA_DTL_PK");

            entity.ToTable("IMMS_RITES_POCA_DTL");

            entity.Property(e => e.Rly)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("RLY");
            entity.Property(e => e.Cakey)
                .HasPrecision(10)
                .HasColumnName("CAKEY");
            entity.Property(e => e.Slno)
                .HasPrecision(2)
                .HasColumnName("SLNO");
            entity.Property(e => e.CancQty)
                .HasColumnType("NUMBER(15,3)")
                .HasColumnName("CANC_QTY");
            entity.Property(e => e.Datetime)
                .HasColumnType("DATE")
                .HasColumnName("DATETIME");
            entity.Property(e => e.DemStatus)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("DEM_STATUS");
            entity.Property(e => e.PlNo)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("PL_NO");
            entity.Property(e => e.PoBalQty)
                .HasColumnType("NUMBER(15,3)")
                .HasColumnName("PO_BAL_QTY");
            entity.Property(e => e.PoSr)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("PO_SR");
            entity.Property(e => e.Status)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("STATUS");
        });

        modelBuilder.Entity<ImmsRitesPocaHdr>(entity =>
        {
            entity.HasKey(e => new { e.Rly, e.Cakey }).HasName("IMMS_RITES_POCA_HDR_PK");

            entity.ToTable("IMMS_RITES_POCA_HDR");

            entity.Property(e => e.Rly)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("RLY");
            entity.Property(e => e.Cakey)
                .HasPrecision(10)
                .HasColumnName("CAKEY");
            entity.Property(e => e.CaDate)
                .HasColumnType("DATE")
                .HasColumnName("CA_DATE");
            entity.Property(e => e.CaNo)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("CA_NO");
            entity.Property(e => e.CaType)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("CA_TYPE");
            entity.Property(e => e.CakeyDate)
                .HasColumnType("DATE")
                .HasColumnName("CAKEY_DATE");
            entity.Property(e => e.Datetime)
                .HasColumnType("DATE")
                .HasColumnName("DATETIME");
            entity.Property(e => e.PoNo)
                .HasMaxLength(14)
                .IsUnicode(false)
                .HasColumnName("PO_NO");
            entity.Property(e => e.Pokey)
                .HasPrecision(10)
                .HasColumnName("POKEY");
            entity.Property(e => e.RefDate)
                .HasColumnType("DATE")
                .HasColumnName("REF_DATE");
            entity.Property(e => e.RefNo)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("REF_NO");
            entity.Property(e => e.Remarks)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .HasColumnName("REMARKS");
            entity.Property(e => e.Vcode)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("VCODE");
        });

        modelBuilder.Entity<ImpSd142175045>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("IMP_SD_142-17_50_45");

            entity.HasIndex(e => new { e.ProcessOrder, e.Duplicate }, "SYS_MTABLE_000012042_IND_1").IsUnique();

            entity.HasIndex(e => new { e.ObjectSchema, e.OriginalObjectName, e.ObjectType }, "SYS_MTABLE_000012042_IND_2");

            entity.HasIndex(e => new { e.ObjectSchema, e.ObjectName, e.ObjectType, e.PartitionName, e.SubpartitionName }, "SYS_MTABLE_000012042_IND_3");

            entity.HasIndex(e => e.BaseProcessOrder, "SYS_MTABLE_000012042_IND_4");

            entity.HasIndex(e => new { e.OriginalObjectSchema, e.OriginalObjectName, e.PartitionName }, "SYS_MTABLE_000012042_IND_5");

            entity.HasIndex(e => e.ObjectPathSeqno, "SYS_MTABLE_000012042_IND_6");

            entity.Property(e => e.AbortStep)
                .HasColumnType("NUMBER")
                .HasColumnName("ABORT_STEP");
            entity.Property(e => e.AccessMethod)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("ACCESS_METHOD");
            entity.Property(e => e.AncestorObjectName)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("ANCESTOR_OBJECT_NAME");
            entity.Property(e => e.AncestorObjectSchema)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("ANCESTOR_OBJECT_SCHEMA");
            entity.Property(e => e.AncestorObjectType)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("ANCESTOR_OBJECT_TYPE");
            entity.Property(e => e.AncestorProcessOrder)
                .HasColumnType("NUMBER")
                .HasColumnName("ANCESTOR_PROCESS_ORDER");
            entity.Property(e => e.BaseObjectName)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("BASE_OBJECT_NAME");
            entity.Property(e => e.BaseObjectSchema)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("BASE_OBJECT_SCHEMA");
            entity.Property(e => e.BaseObjectType)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("BASE_OBJECT_TYPE");
            entity.Property(e => e.BaseProcessOrder)
                .HasColumnType("NUMBER")
                .HasColumnName("BASE_PROCESS_ORDER");
            entity.Property(e => e.BlockSize)
                .HasColumnType("NUMBER")
                .HasColumnName("BLOCK_SIZE");
            entity.Property(e => e.ClusterOk)
                .HasColumnType("NUMBER")
                .HasColumnName("CLUSTER_OK");
            entity.Property(e => e.CompletedBytes)
                .HasColumnType("NUMBER")
                .HasColumnName("COMPLETED_BYTES");
            entity.Property(e => e.CompletedRows)
                .HasColumnType("NUMBER")
                .HasColumnName("COMPLETED_ROWS");
            entity.Property(e => e.CompletionTime)
                .HasColumnType("DATE")
                .HasColumnName("COMPLETION_TIME");
            entity.Property(e => e.ControlQueue)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("CONTROL_QUEUE");
            entity.Property(e => e.CreationLevel)
                .HasColumnType("NUMBER")
                .HasColumnName("CREATION_LEVEL");
            entity.Property(e => e.CreationTime)
                .HasColumnType("DATE")
                .HasColumnName("CREATION_TIME");
            entity.Property(e => e.CumulativeTime)
                .HasColumnType("NUMBER")
                .HasColumnName("CUMULATIVE_TIME");
            entity.Property(e => e.DataBufferSize)
                .HasColumnType("NUMBER")
                .HasColumnName("DATA_BUFFER_SIZE");
            entity.Property(e => e.DataIo)
                .HasColumnType("NUMBER")
                .HasColumnName("DATA_IO");
            entity.Property(e => e.DataobjNum)
                .HasColumnType("NUMBER")
                .HasColumnName("DATAOBJ_NUM");
            entity.Property(e => e.DbVersion)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("DB_VERSION");
            entity.Property(e => e.Degree)
                .HasColumnType("NUMBER")
                .HasColumnName("DEGREE");
            entity.Property(e => e.DomainProcessOrder)
                .HasColumnType("NUMBER")
                .HasColumnName("DOMAIN_PROCESS_ORDER");
            entity.Property(e => e.DumpAllocation)
                .HasColumnType("NUMBER")
                .HasColumnName("DUMP_ALLOCATION");
            entity.Property(e => e.DumpFileid)
                .HasColumnType("NUMBER")
                .HasColumnName("DUMP_FILEID");
            entity.Property(e => e.DumpLength)
                .HasColumnType("NUMBER")
                .HasColumnName("DUMP_LENGTH");
            entity.Property(e => e.DumpOrigLength)
                .HasColumnType("NUMBER")
                .HasColumnName("DUMP_ORIG_LENGTH");
            entity.Property(e => e.DumpPosition)
                .HasColumnType("NUMBER")
                .HasColumnName("DUMP_POSITION");
            entity.Property(e => e.Duplicate)
                .HasColumnType("NUMBER")
                .HasColumnName("DUPLICATE");
            entity.Property(e => e.ElapsedTime)
                .HasColumnType("NUMBER")
                .HasColumnName("ELAPSED_TIME");
            entity.Property(e => e.ErrorCount)
                .HasColumnType("NUMBER")
                .HasColumnName("ERROR_COUNT");
            entity.Property(e => e.ExtendSize)
                .HasColumnType("NUMBER")
                .HasColumnName("EXTEND_SIZE");
            entity.Property(e => e.FileMaxSize)
                .HasColumnType("NUMBER")
                .HasColumnName("FILE_MAX_SIZE");
            entity.Property(e => e.FileName)
                .IsUnicode(false)
                .HasColumnName("FILE_NAME");
            entity.Property(e => e.FileType)
                .HasColumnType("NUMBER")
                .HasColumnName("FILE_TYPE");
            entity.Property(e => e.Flags)
                .HasColumnType("NUMBER")
                .HasColumnName("FLAGS");
            entity.Property(e => e.Grantor)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("GRANTOR");
            entity.Property(e => e.Granules)
                .HasColumnType("NUMBER")
                .HasColumnName("GRANULES");
            entity.Property(e => e.Guid).HasColumnName("GUID");
            entity.Property(e => e.InProgress)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("IN_PROGRESS");
            entity.Property(e => e.Instance)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("INSTANCE");
            entity.Property(e => e.InstanceId)
                .HasColumnType("NUMBER")
                .HasColumnName("INSTANCE_ID");
            entity.Property(e => e.IsDefault)
                .HasColumnType("NUMBER")
                .HasColumnName("IS_DEFAULT");
            entity.Property(e => e.JobMode)
                .HasMaxLength(21)
                .IsUnicode(false)
                .HasColumnName("JOB_MODE");
            entity.Property(e => e.JobVersion)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("JOB_VERSION");
            entity.Property(e => e.LastFile)
                .HasColumnType("NUMBER")
                .HasColumnName("LAST_FILE");
            entity.Property(e => e.LastUpdate)
                .HasColumnType("DATE")
                .HasColumnName("LAST_UPDATE");
            entity.Property(e => e.LoadMethod)
                .HasColumnType("NUMBER")
                .HasColumnName("LOAD_METHOD");
            entity.Property(e => e.MetadataBufferSize)
                .HasColumnType("NUMBER")
                .HasColumnName("METADATA_BUFFER_SIZE");
            entity.Property(e => e.MetadataIo)
                .HasColumnType("NUMBER")
                .HasColumnName("METADATA_IO");
            entity.Property(e => e.Name)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("NAME");
            entity.Property(e => e.ObjectIntOid)
                .HasMaxLength(130)
                .IsUnicode(false)
                .HasColumnName("OBJECT_INT_OID");
            entity.Property(e => e.ObjectLongName)
                .IsUnicode(false)
                .HasColumnName("OBJECT_LONG_NAME");
            entity.Property(e => e.ObjectName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("OBJECT_NAME");
            entity.Property(e => e.ObjectNumber)
                .HasColumnType("NUMBER")
                .HasColumnName("OBJECT_NUMBER");
            entity.Property(e => e.ObjectPathSeqno)
                .HasColumnType("NUMBER")
                .HasColumnName("OBJECT_PATH_SEQNO");
            entity.Property(e => e.ObjectRow)
                .HasColumnType("NUMBER")
                .HasColumnName("OBJECT_ROW");
            entity.Property(e => e.ObjectSchema)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("OBJECT_SCHEMA");
            entity.Property(e => e.ObjectTablespace)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("OBJECT_TABLESPACE");
            entity.Property(e => e.ObjectType)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("OBJECT_TYPE");
            entity.Property(e => e.ObjectTypePath)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("OBJECT_TYPE_PATH");
            entity.Property(e => e.OldValue)
                .IsUnicode(false)
                .HasColumnName("OLD_VALUE");
            entity.Property(e => e.Operation)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("OPERATION");
            entity.Property(e => e.OptionTag)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("OPTION_TAG");
            entity.Property(e => e.OrigBaseObjectName)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("ORIG_BASE_OBJECT_NAME");
            entity.Property(e => e.OrigBaseObjectSchema)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("ORIG_BASE_OBJECT_SCHEMA");
            entity.Property(e => e.OriginalObjectName)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("ORIGINAL_OBJECT_NAME");
            entity.Property(e => e.OriginalObjectSchema)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("ORIGINAL_OBJECT_SCHEMA");
            entity.Property(e => e.PacketNumber)
                .HasColumnType("NUMBER")
                .HasColumnName("PACKET_NUMBER");
            entity.Property(e => e.Parallelization)
                .HasColumnType("NUMBER")
                .HasColumnName("PARALLELIZATION");
            entity.Property(e => e.ParentObjectName)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("PARENT_OBJECT_NAME");
            entity.Property(e => e.ParentObjectSchema)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("PARENT_OBJECT_SCHEMA");
            entity.Property(e => e.ParentProcessOrder)
                .HasColumnType("NUMBER")
                .HasColumnName("PARENT_PROCESS_ORDER");
            entity.Property(e => e.PartitionName)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("PARTITION_NAME");
            entity.Property(e => e.Phase)
                .HasColumnType("NUMBER")
                .HasColumnName("PHASE");
            entity.Property(e => e.Platform)
                .HasMaxLength(101)
                .IsUnicode(false)
                .HasColumnName("PLATFORM");
            entity.Property(e => e.ProcessName)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("PROCESS_NAME");
            entity.Property(e => e.ProcessOrder)
                .HasColumnType("NUMBER")
                .HasColumnName("PROCESS_ORDER");
            entity.Property(e => e.ProcessingState)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("PROCESSING_STATE");
            entity.Property(e => e.ProcessingStatus)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("PROCESSING_STATUS");
            entity.Property(e => e.Property)
                .HasColumnType("NUMBER")
                .HasColumnName("PROPERTY");
            entity.Property(e => e.ProxySchema)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("PROXY_SCHEMA");
            entity.Property(e => e.ProxyView)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("PROXY_VIEW");
            entity.Property(e => e.QueueTabnum)
                .HasColumnType("NUMBER")
                .HasColumnName("QUEUE_TABNUM");
            entity.Property(e => e.RemoteLink)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("REMOTE_LINK");
            entity.Property(e => e.Scn)
                .HasColumnType("NUMBER")
                .HasColumnName("SCN");
            entity.Property(e => e.Seed)
                .HasColumnType("NUMBER")
                .HasColumnName("SEED");
            entity.Property(e => e.ServiceName)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("SERVICE_NAME");
            entity.Property(e => e.SizeEstimate)
                .HasColumnType("NUMBER")
                .HasColumnName("SIZE_ESTIMATE");
            entity.Property(e => e.SrcCompat)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("SRC_COMPAT");
            entity.Property(e => e.StartTime)
                .HasColumnType("DATE")
                .HasColumnName("START_TIME");
            entity.Property(e => e.State)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("STATE");
            entity.Property(e => e.StatusQueue)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("STATUS_QUEUE");
            entity.Property(e => e.SubpartitionName)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("SUBPARTITION_NAME");
            entity.Property(e => e.TargetXmlClob)
                .HasColumnType("CLOB")
                .HasColumnName("TARGET_XML_CLOB");
            entity.Property(e => e.TdeRewrappedKey).HasColumnName("TDE_REWRAPPED_KEY");
            entity.Property(e => e.TemplateTable)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("TEMPLATE_TABLE");
            entity.Property(e => e.Timezone)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("TIMEZONE");
            entity.Property(e => e.TotalBytes)
                .HasColumnType("NUMBER")
                .HasColumnName("TOTAL_BYTES");
            entity.Property(e => e.Trigflag)
                .HasColumnType("NUMBER")
                .HasColumnName("TRIGFLAG");
            entity.Property(e => e.UnloadMethod)
                .HasColumnType("NUMBER")
                .HasColumnName("UNLOAD_METHOD");
            entity.Property(e => e.UserDirectory)
                .IsUnicode(false)
                .HasColumnName("USER_DIRECTORY");
            entity.Property(e => e.UserFileName)
                .IsUnicode(false)
                .HasColumnName("USER_FILE_NAME");
            entity.Property(e => e.UserName)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("USER_NAME");
            entity.Property(e => e.ValueN)
                .HasColumnType("NUMBER")
                .HasColumnName("VALUE_N");
            entity.Property(e => e.ValueT)
                .IsUnicode(false)
                .HasColumnName("VALUE_T");
            entity.Property(e => e.Version)
                .HasColumnType("NUMBER")
                .HasColumnName("VERSION");
            entity.Property(e => e.WorkItem)
                .HasMaxLength(21)
                .IsUnicode(false)
                .HasColumnName("WORK_ITEM");
            entity.Property(e => e.XmlClob)
                .HasColumnType("CLOB")
                .HasColumnName("XML_CLOB");
        });

        modelBuilder.Entity<ImpSd148175452>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("IMP_SD_148-17_54_52");

            entity.HasIndex(e => new { e.ProcessOrder, e.Duplicate }, "SYS_MTABLE_000012072_IND_1").IsUnique();

            entity.HasIndex(e => new { e.ObjectSchema, e.OriginalObjectName, e.ObjectType }, "SYS_MTABLE_000012072_IND_2");

            entity.HasIndex(e => new { e.ObjectSchema, e.ObjectName, e.ObjectType, e.PartitionName, e.SubpartitionName }, "SYS_MTABLE_000012072_IND_3");

            entity.HasIndex(e => e.BaseProcessOrder, "SYS_MTABLE_000012072_IND_4");

            entity.HasIndex(e => new { e.OriginalObjectSchema, e.OriginalObjectName, e.PartitionName }, "SYS_MTABLE_000012072_IND_5");

            entity.HasIndex(e => e.ObjectPathSeqno, "SYS_MTABLE_000012072_IND_6");

            entity.Property(e => e.AbortStep)
                .HasColumnType("NUMBER")
                .HasColumnName("ABORT_STEP");
            entity.Property(e => e.AccessMethod)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("ACCESS_METHOD");
            entity.Property(e => e.AncestorObjectName)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("ANCESTOR_OBJECT_NAME");
            entity.Property(e => e.AncestorObjectSchema)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("ANCESTOR_OBJECT_SCHEMA");
            entity.Property(e => e.AncestorObjectType)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("ANCESTOR_OBJECT_TYPE");
            entity.Property(e => e.AncestorProcessOrder)
                .HasColumnType("NUMBER")
                .HasColumnName("ANCESTOR_PROCESS_ORDER");
            entity.Property(e => e.BaseObjectName)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("BASE_OBJECT_NAME");
            entity.Property(e => e.BaseObjectSchema)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("BASE_OBJECT_SCHEMA");
            entity.Property(e => e.BaseObjectType)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("BASE_OBJECT_TYPE");
            entity.Property(e => e.BaseProcessOrder)
                .HasColumnType("NUMBER")
                .HasColumnName("BASE_PROCESS_ORDER");
            entity.Property(e => e.BlockSize)
                .HasColumnType("NUMBER")
                .HasColumnName("BLOCK_SIZE");
            entity.Property(e => e.ClusterOk)
                .HasColumnType("NUMBER")
                .HasColumnName("CLUSTER_OK");
            entity.Property(e => e.CompletedBytes)
                .HasColumnType("NUMBER")
                .HasColumnName("COMPLETED_BYTES");
            entity.Property(e => e.CompletedRows)
                .HasColumnType("NUMBER")
                .HasColumnName("COMPLETED_ROWS");
            entity.Property(e => e.CompletionTime)
                .HasColumnType("DATE")
                .HasColumnName("COMPLETION_TIME");
            entity.Property(e => e.ControlQueue)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("CONTROL_QUEUE");
            entity.Property(e => e.CreationLevel)
                .HasColumnType("NUMBER")
                .HasColumnName("CREATION_LEVEL");
            entity.Property(e => e.CreationTime)
                .HasColumnType("DATE")
                .HasColumnName("CREATION_TIME");
            entity.Property(e => e.CumulativeTime)
                .HasColumnType("NUMBER")
                .HasColumnName("CUMULATIVE_TIME");
            entity.Property(e => e.DataBufferSize)
                .HasColumnType("NUMBER")
                .HasColumnName("DATA_BUFFER_SIZE");
            entity.Property(e => e.DataIo)
                .HasColumnType("NUMBER")
                .HasColumnName("DATA_IO");
            entity.Property(e => e.DataobjNum)
                .HasColumnType("NUMBER")
                .HasColumnName("DATAOBJ_NUM");
            entity.Property(e => e.DbVersion)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("DB_VERSION");
            entity.Property(e => e.Degree)
                .HasColumnType("NUMBER")
                .HasColumnName("DEGREE");
            entity.Property(e => e.DomainProcessOrder)
                .HasColumnType("NUMBER")
                .HasColumnName("DOMAIN_PROCESS_ORDER");
            entity.Property(e => e.DumpAllocation)
                .HasColumnType("NUMBER")
                .HasColumnName("DUMP_ALLOCATION");
            entity.Property(e => e.DumpFileid)
                .HasColumnType("NUMBER")
                .HasColumnName("DUMP_FILEID");
            entity.Property(e => e.DumpLength)
                .HasColumnType("NUMBER")
                .HasColumnName("DUMP_LENGTH");
            entity.Property(e => e.DumpOrigLength)
                .HasColumnType("NUMBER")
                .HasColumnName("DUMP_ORIG_LENGTH");
            entity.Property(e => e.DumpPosition)
                .HasColumnType("NUMBER")
                .HasColumnName("DUMP_POSITION");
            entity.Property(e => e.Duplicate)
                .HasColumnType("NUMBER")
                .HasColumnName("DUPLICATE");
            entity.Property(e => e.ElapsedTime)
                .HasColumnType("NUMBER")
                .HasColumnName("ELAPSED_TIME");
            entity.Property(e => e.ErrorCount)
                .HasColumnType("NUMBER")
                .HasColumnName("ERROR_COUNT");
            entity.Property(e => e.ExtendSize)
                .HasColumnType("NUMBER")
                .HasColumnName("EXTEND_SIZE");
            entity.Property(e => e.FileMaxSize)
                .HasColumnType("NUMBER")
                .HasColumnName("FILE_MAX_SIZE");
            entity.Property(e => e.FileName)
                .IsUnicode(false)
                .HasColumnName("FILE_NAME");
            entity.Property(e => e.FileType)
                .HasColumnType("NUMBER")
                .HasColumnName("FILE_TYPE");
            entity.Property(e => e.Flags)
                .HasColumnType("NUMBER")
                .HasColumnName("FLAGS");
            entity.Property(e => e.Grantor)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("GRANTOR");
            entity.Property(e => e.Granules)
                .HasColumnType("NUMBER")
                .HasColumnName("GRANULES");
            entity.Property(e => e.Guid).HasColumnName("GUID");
            entity.Property(e => e.InProgress)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("IN_PROGRESS");
            entity.Property(e => e.Instance)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("INSTANCE");
            entity.Property(e => e.InstanceId)
                .HasColumnType("NUMBER")
                .HasColumnName("INSTANCE_ID");
            entity.Property(e => e.IsDefault)
                .HasColumnType("NUMBER")
                .HasColumnName("IS_DEFAULT");
            entity.Property(e => e.JobMode)
                .HasMaxLength(21)
                .IsUnicode(false)
                .HasColumnName("JOB_MODE");
            entity.Property(e => e.JobVersion)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("JOB_VERSION");
            entity.Property(e => e.LastFile)
                .HasColumnType("NUMBER")
                .HasColumnName("LAST_FILE");
            entity.Property(e => e.LastUpdate)
                .HasColumnType("DATE")
                .HasColumnName("LAST_UPDATE");
            entity.Property(e => e.LoadMethod)
                .HasColumnType("NUMBER")
                .HasColumnName("LOAD_METHOD");
            entity.Property(e => e.MetadataBufferSize)
                .HasColumnType("NUMBER")
                .HasColumnName("METADATA_BUFFER_SIZE");
            entity.Property(e => e.MetadataIo)
                .HasColumnType("NUMBER")
                .HasColumnName("METADATA_IO");
            entity.Property(e => e.Name)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("NAME");
            entity.Property(e => e.ObjectIntOid)
                .HasMaxLength(130)
                .IsUnicode(false)
                .HasColumnName("OBJECT_INT_OID");
            entity.Property(e => e.ObjectLongName)
                .IsUnicode(false)
                .HasColumnName("OBJECT_LONG_NAME");
            entity.Property(e => e.ObjectName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("OBJECT_NAME");
            entity.Property(e => e.ObjectNumber)
                .HasColumnType("NUMBER")
                .HasColumnName("OBJECT_NUMBER");
            entity.Property(e => e.ObjectPathSeqno)
                .HasColumnType("NUMBER")
                .HasColumnName("OBJECT_PATH_SEQNO");
            entity.Property(e => e.ObjectRow)
                .HasColumnType("NUMBER")
                .HasColumnName("OBJECT_ROW");
            entity.Property(e => e.ObjectSchema)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("OBJECT_SCHEMA");
            entity.Property(e => e.ObjectTablespace)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("OBJECT_TABLESPACE");
            entity.Property(e => e.ObjectType)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("OBJECT_TYPE");
            entity.Property(e => e.ObjectTypePath)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("OBJECT_TYPE_PATH");
            entity.Property(e => e.OldValue)
                .IsUnicode(false)
                .HasColumnName("OLD_VALUE");
            entity.Property(e => e.Operation)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("OPERATION");
            entity.Property(e => e.OptionTag)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("OPTION_TAG");
            entity.Property(e => e.OrigBaseObjectName)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("ORIG_BASE_OBJECT_NAME");
            entity.Property(e => e.OrigBaseObjectSchema)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("ORIG_BASE_OBJECT_SCHEMA");
            entity.Property(e => e.OriginalObjectName)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("ORIGINAL_OBJECT_NAME");
            entity.Property(e => e.OriginalObjectSchema)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("ORIGINAL_OBJECT_SCHEMA");
            entity.Property(e => e.PacketNumber)
                .HasColumnType("NUMBER")
                .HasColumnName("PACKET_NUMBER");
            entity.Property(e => e.Parallelization)
                .HasColumnType("NUMBER")
                .HasColumnName("PARALLELIZATION");
            entity.Property(e => e.ParentObjectName)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("PARENT_OBJECT_NAME");
            entity.Property(e => e.ParentObjectSchema)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("PARENT_OBJECT_SCHEMA");
            entity.Property(e => e.ParentProcessOrder)
                .HasColumnType("NUMBER")
                .HasColumnName("PARENT_PROCESS_ORDER");
            entity.Property(e => e.PartitionName)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("PARTITION_NAME");
            entity.Property(e => e.Phase)
                .HasColumnType("NUMBER")
                .HasColumnName("PHASE");
            entity.Property(e => e.Platform)
                .HasMaxLength(101)
                .IsUnicode(false)
                .HasColumnName("PLATFORM");
            entity.Property(e => e.ProcessName)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("PROCESS_NAME");
            entity.Property(e => e.ProcessOrder)
                .HasColumnType("NUMBER")
                .HasColumnName("PROCESS_ORDER");
            entity.Property(e => e.ProcessingState)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("PROCESSING_STATE");
            entity.Property(e => e.ProcessingStatus)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("PROCESSING_STATUS");
            entity.Property(e => e.Property)
                .HasColumnType("NUMBER")
                .HasColumnName("PROPERTY");
            entity.Property(e => e.ProxySchema)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("PROXY_SCHEMA");
            entity.Property(e => e.ProxyView)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("PROXY_VIEW");
            entity.Property(e => e.QueueTabnum)
                .HasColumnType("NUMBER")
                .HasColumnName("QUEUE_TABNUM");
            entity.Property(e => e.RemoteLink)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("REMOTE_LINK");
            entity.Property(e => e.Scn)
                .HasColumnType("NUMBER")
                .HasColumnName("SCN");
            entity.Property(e => e.Seed)
                .HasColumnType("NUMBER")
                .HasColumnName("SEED");
            entity.Property(e => e.ServiceName)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("SERVICE_NAME");
            entity.Property(e => e.SizeEstimate)
                .HasColumnType("NUMBER")
                .HasColumnName("SIZE_ESTIMATE");
            entity.Property(e => e.SrcCompat)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("SRC_COMPAT");
            entity.Property(e => e.StartTime)
                .HasColumnType("DATE")
                .HasColumnName("START_TIME");
            entity.Property(e => e.State)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("STATE");
            entity.Property(e => e.StatusQueue)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("STATUS_QUEUE");
            entity.Property(e => e.SubpartitionName)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("SUBPARTITION_NAME");
            entity.Property(e => e.TargetXmlClob)
                .HasColumnType("CLOB")
                .HasColumnName("TARGET_XML_CLOB");
            entity.Property(e => e.TdeRewrappedKey).HasColumnName("TDE_REWRAPPED_KEY");
            entity.Property(e => e.TemplateTable)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("TEMPLATE_TABLE");
            entity.Property(e => e.Timezone)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("TIMEZONE");
            entity.Property(e => e.TotalBytes)
                .HasColumnType("NUMBER")
                .HasColumnName("TOTAL_BYTES");
            entity.Property(e => e.Trigflag)
                .HasColumnType("NUMBER")
                .HasColumnName("TRIGFLAG");
            entity.Property(e => e.UnloadMethod)
                .HasColumnType("NUMBER")
                .HasColumnName("UNLOAD_METHOD");
            entity.Property(e => e.UserDirectory)
                .IsUnicode(false)
                .HasColumnName("USER_DIRECTORY");
            entity.Property(e => e.UserFileName)
                .IsUnicode(false)
                .HasColumnName("USER_FILE_NAME");
            entity.Property(e => e.UserName)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("USER_NAME");
            entity.Property(e => e.ValueN)
                .HasColumnType("NUMBER")
                .HasColumnName("VALUE_N");
            entity.Property(e => e.ValueT)
                .IsUnicode(false)
                .HasColumnName("VALUE_T");
            entity.Property(e => e.Version)
                .HasColumnType("NUMBER")
                .HasColumnName("VERSION");
            entity.Property(e => e.WorkItem)
                .HasMaxLength(21)
                .IsUnicode(false)
                .HasColumnName("WORK_ITEM");
            entity.Property(e => e.XmlClob)
                .HasColumnType("CLOB")
                .HasColumnName("XML_CLOB");
        });

        modelBuilder.Entity<IndiaPinCode>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("INDIA_PIN_CODE");

            entity.Property(e => e.Location)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("LOCATION");
            entity.Property(e => e.Pincode)
                .HasPrecision(10)
                .HasColumnName("PINCODE");
            entity.Property(e => e.State)
                .HasMaxLength(26)
                .IsUnicode(false)
                .HasColumnName("STATE");
        });

        modelBuilder.Entity<InspectionTestParamvalue>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("INSPECTION_TEST_PARAMVALUE");

            entity.Property(e => e.GrpOrder)
                .HasColumnType("NUMBER")
                .HasColumnName("GRP_ORDER");
            entity.Property(e => e.GrpOrderId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("GRP_ORDER_ID");
            entity.Property(e => e.HeaderDisplay)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("HEADER_DISPLAY");
            entity.Property(e => e.InsptHead)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("INSPT_HEAD");
            entity.Property(e => e.InsptSno)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("INSPT_SNO");
            entity.Property(e => e.ItemCd)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("ITEM_CD");
            entity.Property(e => e.Parmeter)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("PARMETER");
            entity.Property(e => e.Remarks)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .HasColumnName("REMARKS");
            entity.Property(e => e.SrNo)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("SR_NO");
            entity.Property(e => e.ValueSpecified)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .HasColumnName("VALUE_SPECIFIED");
        });

        modelBuilder.Entity<InspectionTestPln>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("INSPECTION_TEST_PLN");

            entity.Property(e => e.BkNo)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("BK_NO");
            entity.Property(e => e.CalibrReport)
                .HasColumnType("NUMBER")
                .HasColumnName("CALIBR_REPORT");
            entity.Property(e => e.CallRecvDt)
                .HasColumnType("DATE")
                .HasColumnName("CALL_RECV_DT");
            entity.Property(e => e.CallSno)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("CALL_SNO");
            entity.Property(e => e.CaseNo)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("CASE_NO");
            entity.Property(e => e.DatasheetRemarks1)
                .IsUnicode(false)
                .HasColumnName("DATASHEET_REMARKS1");
            entity.Property(e => e.DatasheetRemarks2)
                .IsUnicode(false)
                .HasColumnName("DATASHEET_REMARKS2");
            entity.Property(e => e.DatasheetRemarks3)
                .IsUnicode(false)
                .HasColumnName("DATASHEET_REMARKS3");
            entity.Property(e => e.DatasheetRemarks4)
                .IsUnicode(false)
                .HasColumnName("DATASHEET_REMARKS4");
            entity.Property(e => e.DatasheetRemarks5).HasColumnName("DATASHEET_REMARKS5");
            entity.Property(e => e.DateOfInspect)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("DATE_OF_INSPECT");
            entity.Property(e => e.DocumentVeri)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("DOCUMENT_VERI");
            entity.Property(e => e.DrawingSpe)
                .IsUnicode(false)
                .HasColumnName("DRAWING_SPE");
            entity.Property(e => e.Extracolum1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("EXTRACOLUM1");
            entity.Property(e => e.Extracolum2)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("EXTRACOLUM2");
            entity.Property(e => e.Extracolum3)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("EXTRACOLUM3");
            entity.Property(e => e.Extracolum4)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("EXTRACOLUM4");
            entity.Property(e => e.Extracolum5)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("EXTRACOLUM5");
            entity.Property(e => e.Extracolum6)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("EXTRACOLUM6");
            entity.Property(e => e.Extracolum7)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("EXTRACOLUM7");
            entity.Property(e => e.Extracolum8)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("EXTRACOLUM8");
            entity.Property(e => e.Extracolum9)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("EXTRACOLUM9");
            entity.Property(e => e.FilePath)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("FILE_PATH");
            entity.Property(e => e.FilePath2)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("FILE_PATH2");
            entity.Property(e => e.FilePath3)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("FILE_PATH3");
            entity.Property(e => e.FilePath4)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("FILE_PATH4");
            entity.Property(e => e.FilePath5)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("FILE_PATH5");
            entity.Property(e => e.FinalStatement)
                .IsUnicode(false)
                .HasColumnName("FINAL_STATEMENT");
            entity.Property(e => e.FinalStatementStatus)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("FINAL_STATEMENT_STATUS");
            entity.Property(e => e.FinalSubmit)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("FINAL_SUBMIT");
            entity.Property(e => e.IeCd)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("IE_CD");
            entity.Property(e => e.InspectCd)
                .HasMaxLength(30)
                .HasColumnName("INSPECT_CD");
            entity.Property(e => e.IntExtTstReport)
                .HasColumnType("NUMBER")
                .HasColumnName("INT_EXT_TST_REPORT");
            entity.Property(e => e.InttstReport)
                .HasColumnType("NUMBER")
                .HasColumnName("INTTST_REPORT");
            entity.Property(e => e.ItemCd)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("ITEM_CD");
            entity.Property(e => e.ManualEntry)
                .HasColumnType("NUMBER")
                .HasColumnName("MANUAL_ENTRY");
            entity.Property(e => e.NoItemInspected)
                .HasColumnType("NUMBER")
                .HasColumnName("NO_ITEM_INSPECTED");
            entity.Property(e => e.PlNo)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("PL_NO");
            entity.Property(e => e.PlaceOfInspect)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("PLACE_OF_INSPECT");
            entity.Property(e => e.PoNo)
                .HasMaxLength(75)
                .IsUnicode(false)
                .HasColumnName("PO_NO");
            entity.Property(e => e.Range)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("RANGE");
            entity.Property(e => e.RdsoApprReport)
                .HasColumnType("NUMBER")
                .HasColumnName("RDSO_APPR_REPORT");
            entity.Property(e => e.RdsoApproSplr)
                .HasColumnType("NUMBER")
                .HasColumnName("RDSO_APPRO_SPLR");
            entity.Property(e => e.ReportHeader)
                .IsUnicode(false)
                .HasColumnName("REPORT_HEADER");
            entity.Property(e => e.SetNo)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("SET_NO");
            entity.Property(e => e.SizeOfLot)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("SIZE_OF_LOT");
            entity.Property(e => e.SizeOfSample)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("SIZE_OF_SAMPLE");
        });

        modelBuilder.Entity<InspectionTestPlnTran>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("INSPECTION_TEST_PLN_TRAN");

            entity.Property(e => e.InspectCd)
                .HasMaxLength(30)
                .HasColumnName("INSPECT_CD");
            entity.Property(e => e.InsptHead)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("INSPT_HEAD");
            entity.Property(e => e.InsptSno)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("INSPT_SNO");
            entity.Property(e => e.ItemCd)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("ITEM_CD");
            entity.Property(e => e.ObservStatus)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("OBSERV_STATUS");
            entity.Property(e => e.Observation)
                .IsUnicode(false)
                .HasColumnName("OBSERVATION");
            entity.Property(e => e.Parmeter)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .HasColumnName("PARMETER");
            entity.Property(e => e.Remarks)
                .IsUnicode(false)
                .HasColumnName("REMARKS");
            entity.Property(e => e.ValueSpecified)
                .IsUnicode(false)
                .HasColumnName("VALUE_SPECIFIED");
        });

        modelBuilder.Entity<InspectionTestPlndocVerify>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("INSPECTION_TEST_PLNDOC_VERIFY");

            entity.Property(e => e.Heading)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("HEADING");
            entity.Property(e => e.InspectCd)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("INSPECT_CD");
            entity.Property(e => e.IsChecked)
                .HasMaxLength(20)
                .HasColumnName("IS_CHECKED");
        });

        modelBuilder.Entity<InspectionTstPlnDimension>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("INSPECTION_TST_PLN_DIMENSION");

            entity.Property(e => e.BarPnCd)
                .HasPrecision(10)
                .HasColumnName("BAR_PN_CD");
            entity.Property(e => e.BarPnDia17mm)
                .HasPrecision(10)
                .HasColumnName("BAR_PN_DIA17MM");
            entity.Property(e => e.BarPnTotalLngth)
                .HasPrecision(10)
                .HasColumnName("BAR_PN_TOTAL_LNGTH");
            entity.Property(e => e.CompressedHgt)
                .HasPrecision(10)
                .HasColumnName("COMPRESSED_HGT");
            entity.Property(e => e.DustCvrDiameter)
                .HasPrecision(10)
                .HasColumnName("DUST_CVR_DIAMETER");
            entity.Property(e => e.ExtendedHgt)
                .HasPrecision(10)
                .HasColumnName("EXTENDED_HGT");
            entity.Property(e => e.EyeRing)
                .HasPrecision(10)
                .HasColumnName("EYE_RING");
            entity.Property(e => e.InspectCd)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("INSPECT_CD");
            entity.Property(e => e.PlNo)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("PL_NO");
            entity.Property(e => e.TubeDiameter)
                .HasPrecision(10)
                .HasColumnName("TUBE_DIAMETER");
        });

        modelBuilder.Entity<InspectionTstPlndim>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("INSPECTION_TST_PLNDIM");

            entity.Property(e => e.DimPerDrg)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("DIM_PER_DRG");
            entity.Property(e => e.Dimension)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("DIMENSION");
            entity.Property(e => e.InspectCd)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("INSPECT_CD");
            entity.Property(e => e.NoOfSample)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("NO_OF_SAMPLE");
            entity.Property(e => e.Tolerance)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("TOLERANCE");
        });

        modelBuilder.Entity<InspectionTstPlndimTran>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("INSPECTION_TST_PLNDIM_TRAN");

            entity.Property(e => e.DimCd)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("DIM_CD");
            entity.Property(e => e.Dimparameter)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .HasColumnName("DIMPARAMETER");
            entity.Property(e => e.Dimspecified)
                .HasMaxLength(1200)
                .IsUnicode(false)
                .HasColumnName("DIMSPECIFIED");
            entity.Property(e => e.Dimstatus)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("DIMSTATUS");
            entity.Property(e => e.Dimvalues)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("DIMVALUES");
            entity.Property(e => e.HeaderCode)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("HEADER_CODE");
            entity.Property(e => e.Id)
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.InspectCd)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("INSPECT_CD");
        });

        modelBuilder.Entity<InspectionTstPlndimension1>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("INSPECTION_TST_PLNDIMENSION");

            entity.Property(e => e.DimCd)
                .HasMaxLength(20)
                .HasColumnName("DIM_CD");
            entity.Property(e => e.HeaderCode)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("HEADER_CODE");
            entity.Property(e => e.ItemCd)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ITEM_CD");
            entity.Property(e => e.Parameter)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("PARAMETER");
            entity.Property(e => e.RowHeight)
                .HasColumnType("NUMBER")
                .HasColumnName("ROW_HEIGHT");
            entity.Property(e => e.Specified)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("SPECIFIED");
        });

        modelBuilder.Entity<Mastertablestatus>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("MASTERTABLESTATUS");

            entity.Property(e => e.Tablename)
                .HasMaxLength(26)
                .IsUnicode(false)
                .HasColumnName("TABLENAME");
            entity.Property(e => e.Totalcount)
                .HasColumnType("NUMBER")
                .HasColumnName("TOTALCOUNT");
        });

        modelBuilder.Entity<Migration>(entity =>
        {
            entity.ToTable("MIGRATIONS");

            entity.Property(e => e.Id)
                .HasPrecision(10)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Batch)
                .HasPrecision(11)
                .HasColumnName("BATCH");
            entity.Property(e => e.Migration1)
                .HasMaxLength(191)
                .IsUnicode(false)
                .HasColumnName("MIGRATION");
        });

        modelBuilder.Entity<MmpPomaDtl>(entity =>
        {
            entity.HasKey(e => new { e.Rly, e.Makey, e.Slno }).HasName("PK_POMA_DTL");

            entity.ToTable("MMP_POMA_DTL");

            entity.Property(e => e.Rly)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("RLY");
            entity.Property(e => e.Makey)
                .HasPrecision(10)
                .HasColumnName("MAKEY");
            entity.Property(e => e.Slno)
                .HasPrecision(2)
                .HasColumnName("SLNO");
            entity.Property(e => e.ApprovedBy)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("APPROVED_BY");
            entity.Property(e => e.ApprovedDatetime)
                .HasColumnType("DATE")
                .HasColumnName("APPROVED_DATETIME");
            entity.Property(e => e.CondCode)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("COND_CODE");
            entity.Property(e => e.CondNo)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("COND_NO");
            entity.Property(e => e.CondSlno)
                .HasPrecision(3)
                .HasColumnName("COND_SLNO");
            entity.Property(e => e.ExpCode)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("EXP_CODE");
            entity.Property(e => e.ExpSr)
                .HasPrecision(2)
                .HasColumnName("EXP_SR");
            entity.Property(e => e.MaFld)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("MA_FLD");
            entity.Property(e => e.MaFldDescr)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("MA_FLD_DESCR");
            entity.Property(e => e.MaSrNo)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("MA_SR_NO");
            entity.Property(e => e.MaStatus)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("MA_STATUS");
            entity.Property(e => e.NewValue)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .HasColumnName("NEW_VALUE");
            entity.Property(e => e.NewValueFlag)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("NEW_VALUE_FLAG");
            entity.Property(e => e.NewValueInd)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("NEW_VALUE_IND");
            entity.Property(e => e.OldValue)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .HasColumnName("OLD_VALUE");
            entity.Property(e => e.OrigDp)
                .HasColumnType("DATE")
                .HasColumnName("ORIG_DP");
            entity.Property(e => e.PaymentYear)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("PAYMENT_YEAR");
            entity.Property(e => e.PlNo)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("PL_NO");
            entity.Property(e => e.PoSr)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("PO_SR");
            entity.Property(e => e.Status)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("STATUS");
        });

        modelBuilder.Entity<MmpPomaHdr>(entity =>
        {
            entity.HasKey(e => new { e.Rly, e.Makey }).HasName("PK_POMA_HDR");

            entity.ToTable("MMP_POMA_HDR");

            entity.Property(e => e.Rly)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("RLY");
            entity.Property(e => e.Makey)
                .HasPrecision(10)
                .HasColumnName("MAKEY");
            entity.Property(e => e.AuthSeq)
                .HasPrecision(12)
                .HasColumnName("AUTH_SEQ");
            entity.Property(e => e.AuthSeqFin)
                .HasPrecision(12)
                .HasColumnName("AUTH_SEQ_FIN");
            entity.Property(e => e.Curuser)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("CURUSER");
            entity.Property(e => e.CuruserInd)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("CURUSER_IND");
            entity.Property(e => e.FinStatus)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("FIN_STATUS");
            entity.Property(e => e.Flag)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("FLAG");
            entity.Property(e => e.MaDate)
                .HasColumnType("DATE")
                .HasColumnName("MA_DATE");
            entity.Property(e => e.MaNo)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("MA_NO");
            entity.Property(e => e.MaSignOff)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("MA_SIGN_OFF");
            entity.Property(e => e.MaType)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MA_TYPE");
            entity.Property(e => e.MakeyDate)
                .HasColumnType("DATE")
                .HasColumnName("MAKEY_DATE");
            entity.Property(e => e.NewPoValue)
                .HasPrecision(12)
                .HasColumnName("NEW_PO_VALUE");
            entity.Property(e => e.OldPoValue)
                .HasPrecision(12)
                .HasColumnName("OLD_PO_VALUE");
            entity.Property(e => e.PoMaSrno)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("PO_MA_SRNO");
            entity.Property(e => e.PoNo)
                .HasMaxLength(14)
                .IsUnicode(false)
                .HasColumnName("PO_NO");
            entity.Property(e => e.Pokey)
                .HasPrecision(10)
                .HasColumnName("POKEY");
            entity.Property(e => e.PublishFlag)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("PUBLISH_FLAG");
            entity.Property(e => e.PurDiv)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("PUR_DIV");
            entity.Property(e => e.PurSec)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("PUR_SEC");
            entity.Property(e => e.RecInd)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("REC_IND");
            entity.Property(e => e.RefDate)
                .HasColumnType("DATE")
                .HasColumnName("REF_DATE");
            entity.Property(e => e.RefNo)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("REF_NO");
            entity.Property(e => e.Remarks)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .HasColumnName("REMARKS");
            entity.Property(e => e.ReqId)
                .HasPrecision(12)
                .HasColumnName("REQ_ID");
            entity.Property(e => e.RequestId)
                .HasPrecision(10)
                .HasColumnName("REQUEST_ID");
            entity.Property(e => e.Sent4vet)
                .HasColumnType("DATE")
                .HasColumnName("SENT4VET");
            entity.Property(e => e.SignId)
                .HasPrecision(12)
                .HasColumnName("SIGN_ID");
            entity.Property(e => e.Status)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("STATUS");
            entity.Property(e => e.Subject)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("SUBJECT");
            entity.Property(e => e.Vcode)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("VCODE");
            entity.Property(e => e.VetBy)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("VET_BY");
            entity.Property(e => e.VetDate)
                .HasColumnType("DATE")
                .HasColumnName("VET_DATE");
        });

        modelBuilder.Entity<Myview>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("MYVIEW");

            entity.Property(e => e.CaseNo)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CASE_NO");
            entity.Property(e => e.IeDepartment)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("IE_DEPARTMENT");
            entity.Property(e => e.PoValue)
                .HasColumnType("NUMBER")
                .HasColumnName("PO_VALUE");
            entity.Property(e => e.RegionCode)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("REGION_CODE");
        });

        modelBuilder.Entity<NoIeWorkPlan>(entity =>
        {
            entity.HasKey(e => new { e.IeCd, e.NwpDt });

            entity.ToTable("NO_IE_WORK_PLAN");

            entity.Property(e => e.IeCd)
                .HasPrecision(6)
                .HasColumnName("IE_CD");
            entity.Property(e => e.NwpDt)
                .HasColumnType("DATE")
                .HasColumnName("NWP_DT");
            entity.Property(e => e.CoCd)
                .HasPrecision(3)
                .HasColumnName("CO_CD");
            entity.Property(e => e.Datetime)
                .HasColumnType("DATE")
                .HasColumnName("DATETIME");
            entity.Property(e => e.Reason)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("REASON");
            entity.Property(e => e.RegionCode)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("REGION_CODE");
            entity.Property(e => e.UserId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("USER_ID");

            entity.HasOne(d => d.IeCdNavigation).WithMany(p => p.NoIeWorkPlans)
                .HasForeignKey(d => d.IeCd)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_NWP_IE_CD");
        });

        modelBuilder.Entity<OauthAccessToken>(entity =>
        {
            entity.ToTable("OAUTH_ACCESS_TOKENS");

            entity.Property(e => e.Id)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.ClientId)
                .HasPrecision(10)
                .HasColumnName("CLIENT_ID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("NULL")
                .HasColumnType("DATE")
                .HasColumnName("CREATED_AT");
            entity.Property(e => e.ExpiresAt)
                .HasDefaultValueSql("NULL")
                .HasColumnType("DATE")
                .HasColumnName("EXPIRES_AT");
            entity.Property(e => e.Name)
                .HasMaxLength(191)
                .IsUnicode(false)
                .HasDefaultValueSql("NULL")
                .HasColumnName("NAME");
            entity.Property(e => e.Revoked)
                .HasPrecision(2)
                .HasColumnName("REVOKED");
            entity.Property(e => e.Scopes)
                .IsUnicode(false)
                .HasColumnName("SCOPES");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("NULL")
                .HasColumnType("DATE")
                .HasColumnName("UPDATED_AT");
            entity.Property(e => e.UserId)
                .HasPrecision(11)
                .HasDefaultValueSql("NULL")
                .HasColumnName("USER_ID");
        });

        modelBuilder.Entity<OauthAuthCode>(entity =>
        {
            entity.ToTable("OAUTH_AUTH_CODES");

            entity.Property(e => e.Id)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.ClientId)
                .HasPrecision(10)
                .HasColumnName("CLIENT_ID");
            entity.Property(e => e.ExpiresAt)
                .HasDefaultValueSql("NULL")
                .HasColumnType("DATE")
                .HasColumnName("EXPIRES_AT");
            entity.Property(e => e.Revoked)
                .HasPrecision(1)
                .HasColumnName("REVOKED");
            entity.Property(e => e.Scopes)
                .IsUnicode(false)
                .HasColumnName("SCOPES");
            entity.Property(e => e.UserId)
                .HasPrecision(11)
                .HasColumnName("USER_ID");
        });

        modelBuilder.Entity<OauthClient>(entity =>
        {
            entity.ToTable("OAUTH_CLIENTS");

            entity.Property(e => e.Id)
                .HasPrecision(10)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("NULL")
                .HasColumnType("DATE")
                .HasColumnName("CREATED_AT");
            entity.Property(e => e.Name)
                .HasMaxLength(191)
                .IsUnicode(false)
                .HasColumnName("NAME");
            entity.Property(e => e.PasswordClient)
                .HasPrecision(1)
                .HasColumnName("PASSWORD_CLIENT");
            entity.Property(e => e.PersonalAccessClient)
                .HasPrecision(1)
                .HasColumnName("PERSONAL_ACCESS_CLIENT");
            entity.Property(e => e.Redirect)
                .IsUnicode(false)
                .HasColumnName("REDIRECT");
            entity.Property(e => e.Revoked)
                .HasPrecision(1)
                .HasColumnName("REVOKED");
            entity.Property(e => e.Secret)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("SECRET");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("NULL")
                .HasColumnType("DATE")
                .HasColumnName("UPDATED_AT");
            entity.Property(e => e.UserId)
                .HasPrecision(11)
                .HasDefaultValueSql("NULL")
                .HasColumnName("USER_ID");
        });

        modelBuilder.Entity<OauthPersonalAccessClient>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_OP_ACCESS_CLIENTS");

            entity.ToTable("OAUTH_PERSONAL_ACCESS_CLIENTS");

            entity.Property(e => e.Id)
                .HasPrecision(10)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.ClientId)
                .HasPrecision(10)
                .HasColumnName("CLIENT_ID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("NULL")
                .HasColumnType("DATE")
                .HasColumnName("CREATED_AT");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("NULL")
                .HasColumnType("DATE")
                .HasColumnName("UPDATED_AT");
        });

        modelBuilder.Entity<OauthRefreshToken>(entity =>
        {
            entity.ToTable("OAUTH_REFRESH_TOKENS");

            entity.Property(e => e.Id)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.AccessTokenId)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("ACCESS_TOKEN_ID");
            entity.Property(e => e.ExpiresAt)
                .HasDefaultValueSql("NULL")
                .HasColumnType("DATE")
                .HasColumnName("EXPIRES_AT");
            entity.Property(e => e.Revoked)
                .HasPrecision(1)
                .HasColumnName("REVOKED");
        });

        modelBuilder.Entity<OldSystemOutstanding>(entity =>
        {
            entity.HasKey(e => e.BpoCd).HasName("PK_OS_BPO_CD");

            entity.ToTable("OLD_SYSTEM_OUTSTANDING");

            entity.Property(e => e.BpoCd)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("BPO_CD");
            entity.Property(e => e.OldSystemBpo)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("OLD_SYSTEM_BPO");
            entity.Property(e => e.OutstandingAmt)
                .HasColumnType("NUMBER(13,2)")
                .HasColumnName("OUTSTANDING_AMT");
        });

        modelBuilder.Entity<OngoingNonrlyContract>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("ONGOING_NONRLY_CONTRACTS");

            entity.Property(e => e.ClientName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CLIENT_NAME");
            entity.Property(e => e.ClientSname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CLIENT_SNAME");
            entity.Property(e => e.ClientType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CLIENT_TYPE");
            entity.Property(e => e.ContCm)
                .HasPrecision(3)
                .HasColumnName("CONT_CM");
            entity.Property(e => e.ContFee)
                .HasColumnType("NUMBER(11,4)")
                .HasColumnName("CONT_FEE");
            entity.Property(e => e.ContFeeType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CONT_FEE_TYPE");
            entity.Property(e => e.ContPenalty)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("CONT_PENALTY");
            entity.Property(e => e.ContPeriodFr)
                .HasColumnType("DATE")
                .HasColumnName("CONT_PERIOD_FR");
            entity.Property(e => e.ContPeriodTo)
                .HasColumnType("DATE")
                .HasColumnName("CONT_PERIOD_TO");
            entity.Property(e => e.ContRegion)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CONT_REGION");
            entity.Property(e => e.ContSpecConds)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("CONT_SPEC_CONDS");
            entity.Property(e => e.ContTaxType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CONT_TAX_TYPE");
            entity.Property(e => e.ContractCd)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("CONTRACT_CD");
            entity.Property(e => e.ContractNo)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("CONTRACT_NO");
            entity.Property(e => e.Datetime)
                .HasColumnType("DATE")
                .HasColumnName("DATETIME");
            entity.Property(e => e.MaxFee)
                .HasPrecision(11)
                .HasColumnName("MAX_FEE");
            entity.Property(e => e.MinFee)
                .HasPrecision(11)
                .HasColumnName("MIN_FEE");
            entity.Property(e => e.UserId)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("USER_ID");
        });

        modelBuilder.Entity<OnlinePayment>(entity =>
        {
            entity.HasKey(e => e.MerTxnRef);

            entity.ToTable("ONLINE_PAYMENT");

            entity.Property(e => e.MerTxnRef)
                .HasMaxLength(13)
                .IsUnicode(false)
                .HasColumnName("MER_TXN_REF");
            entity.Property(e => e.Amount)
                .HasColumnType("NUMBER(12,2)")
                .HasColumnName("AMOUNT");
            entity.Property(e => e.AuthCd)
                .HasMaxLength(7)
                .IsUnicode(false)
                .HasColumnName("AUTH_CD");
            entity.Property(e => e.CallRecvDt)
                .HasColumnType("DATE")
                .HasColumnName("CALL_RECV_DT");
            entity.Property(e => e.CallSno)
                .HasPrecision(5)
                .HasColumnName("CALL_SNO");
            entity.Property(e => e.CaseNo)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CASE_NO");
            entity.Property(e => e.ChargesType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CHARGES_TYPE");
            entity.Property(e => e.Datetime)
                .HasColumnType("DATE")
                .HasColumnName("DATETIME");
            entity.Property(e => e.OrderInfo)
                .HasPrecision(5)
                .HasColumnName("ORDER_INFO");
            entity.Property(e => e.RrnNo)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("RRN_NO");
            entity.Property(e => e.Status)
                .HasMaxLength(75)
                .IsUnicode(false)
                .HasColumnName("STATUS");
            entity.Property(e => e.TransactionNo)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("TRANSACTION_NO");
            entity.Property(e => e.VendCd)
                .HasPrecision(6)
                .HasColumnName("VEND_CD");
        });

        modelBuilder.Entity<PasswordReset>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("PASSWORD_RESETS");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("NULL")
                .HasColumnType("DATE")
                .HasColumnName("CREATED_AT");
            entity.Property(e => e.Email)
                .HasMaxLength(191)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.Token)
                .HasMaxLength(191)
                .IsUnicode(false)
                .HasColumnName("TOKEN");
        });

        modelBuilder.Entity<R25R26>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("R25_R26");

            entity.Property(e => e.BankCd)
                .HasPrecision(3)
                .HasColumnName("BANK_CD");
            entity.Property(e => e.BillNo)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("BILL_NO");
            entity.Property(e => e.BpoCd)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("BPO_CD");
            entity.Property(e => e.BpoType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("BPO_TYPE");
            entity.Property(e => e.CaseNo)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CASE_NO");
            entity.Property(e => e.ChqDt)
                .HasColumnType("DATE")
                .HasColumnName("CHQ_DT");
            entity.Property(e => e.ChqNo)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("CHQ_NO");
        });

        modelBuilder.Entity<R29>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("R29");

            entity.Property(e => e.Amt)
                .HasColumnType("NUMBER")
                .HasColumnName("AMT");
            entity.Property(e => e.BankCd)
                .HasPrecision(3)
                .HasColumnName("BANK_CD");
            entity.Property(e => e.ChqDt)
                .HasColumnType("DATE")
                .HasColumnName("CHQ_DT");
            entity.Property(e => e.ChqNo)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("CHQ_NO");
            entity.Property(e => e.Region)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("REGION");
        });

        modelBuilder.Entity<ReturnedBillsBpoChange>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("RETURNED_BILLS_BPO_CHANGE");

            entity.Property(e => e.BillNo)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("BILL_NO");
            entity.Property(e => e.Datetime)
                .HasColumnType("DATE")
                .HasColumnName("DATETIME");
            entity.Property(e => e.NewAccGroup)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("NEW_ACC_GROUP");
            entity.Property(e => e.NewBpoCd)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("NEW_BPO_CD");
            entity.Property(e => e.NewIrfcBpoCd)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("NEW_IRFC_BPO_CD");
            entity.Property(e => e.NewRecipientGstinNo)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("NEW_RECIPIENT_GSTIN_NO");
            entity.Property(e => e.OldAccGroup)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("OLD_ACC_GROUP");
            entity.Property(e => e.OldBpoCd)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("OLD_BPO_CD");
            entity.Property(e => e.OldIrfcBpoCd)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("OLD_IRFC_BPO_CD");
            entity.Property(e => e.OldRecipientGstinNo)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("OLD_RECIPIENT_GSTIN_NO");
            entity.Property(e => e.UserId)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("USER_ID");
        });

        modelBuilder.Entity<RitesBillDtl>(entity =>
        {
            entity.HasKey(e => new { e.BillNo, e.BillResentCount }).HasName("PK_RITES_BILL_NO");

            entity.ToTable("RITES_BILL_DTLS");

            entity.Property(e => e.BillNo)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("BILL_NO");
            entity.Property(e => e.BillResentCount)
                .HasPrecision(1)
                .HasColumnName("BILL_RESENT_COUNT");
            entity.Property(e => e.Amount)
                .HasColumnType("NUMBER(12,2)")
                .HasColumnName("AMOUNT");
            entity.Property(e => e.Au)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("AU");
            entity.Property(e => e.BankAcctNo)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("BANK_ACCT_NO");
            entity.Property(e => e.BankName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("BANK_NAME");
            entity.Property(e => e.Bankaddress)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("BANKADDRESS");
            entity.Property(e => e.BasicValue)
                .HasColumnType("NUMBER(14,3)")
                .HasColumnName("BASIC_VALUE");
            entity.Property(e => e.Billdesc)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("BILLDESC");
            entity.Property(e => e.BkNo)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("BK_NO");
            entity.Property(e => e.Bookdate)
                .HasColumnType("DATE")
                .HasColumnName("BOOKDATE");
            entity.Property(e => e.BpoAdd)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("BPO_ADD");
            entity.Property(e => e.BpoCd)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("BPO_CD");
            entity.Property(e => e.BpoCity)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("BPO_CITY");
            entity.Property(e => e.BpoName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("BPO_NAME");
            entity.Property(e => e.BpoOrgn)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("BPO_ORGN");
            entity.Property(e => e.BpoRly)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("BPO_RLY");
            entity.Property(e => e.BpoType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("BPO_TYPE");
            entity.Property(e => e.CallInstalmentNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CALL_INSTALMENT_NO");
            entity.Property(e => e.CaseNo)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CASE_NO");
            entity.Property(e => e.Cgstamt)
                .HasColumnType("NUMBER")
                .HasColumnName("CGSTAMT");
            entity.Property(e => e.Cgstrate)
                .HasColumnType("NUMBER")
                .HasColumnName("CGSTRATE");
            entity.Property(e => e.Co6Date)
                .HasColumnType("DATE")
                .HasColumnName("CO6_DATE");
            entity.Property(e => e.Co6No)
                .HasMaxLength(14)
                .IsUnicode(false)
                .HasColumnName("CO6_NO");
            entity.Property(e => e.Co6Status)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CO6_STATUS");
            entity.Property(e => e.Co6StatusDate)
                .HasColumnType("DATE")
                .HasColumnName("CO6_STATUS_DATE");
            entity.Property(e => e.Co7Date)
                .HasColumnType("DATE")
                .HasColumnName("CO7_DATE");
            entity.Property(e => e.Co7No)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("CO7_NO");
            entity.Property(e => e.Compositetaxable)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("COMPOSITETAXABLE");
            entity.Property(e => e.Consignee)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("CONSIGNEE");
            entity.Property(e => e.ConsigneeAdd1)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("CONSIGNEE_ADD1");
            entity.Property(e => e.ConsigneeAdd2)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("CONSIGNEE_ADD2");
            entity.Property(e => e.ConsigneeCd)
                .HasPrecision(8)
                .HasColumnName("CONSIGNEE_CD");
            entity.Property(e => e.ConsigneeCity)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CONSIGNEE_CITY");
            entity.Property(e => e.DeductedAmt)
                .HasColumnType("NUMBER(12,2)")
                .HasColumnName("DEDUCTED_AMT");
            entity.Property(e => e.Gsttaxableamt)
                .HasColumnType("NUMBER")
                .HasColumnName("GSTTAXABLEAMT");
            entity.Property(e => e.Gsttdsdeduction)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("GSTTDSDEDUCTION");
            entity.Property(e => e.Hsnsac)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("HSNSAC");
            entity.Property(e => e.Hsnsaccode)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("HSNSACCODE");
            entity.Property(e => e.IcDt)
                .HasColumnType("DATE")
                .HasColumnName("IC_DT");
            entity.Property(e => e.IcNo)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("IC_NO");
            entity.Property(e => e.IcPdf)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("IC_PDF");
            entity.Property(e => e.IeCd)
                .HasPrecision(4)
                .HasColumnName("IE_CD");
            entity.Property(e => e.IeCoCd)
                .HasPrecision(3)
                .HasColumnName("IE_CO_CD");
            entity.Property(e => e.Ifsccode)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("IFSCCODE");
            entity.Property(e => e.Igstamt)
                .HasColumnType("NUMBER")
                .HasColumnName("IGSTAMT");
            entity.Property(e => e.Igstrate)
                .HasColumnType("NUMBER")
                .HasColumnName("IGSTRATE");
            entity.Property(e => e.InvoicePdf)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("INVOICE_PDF");
            entity.Property(e => e.InvoiceSuppDocs)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("INVOICE_SUPP_DOCS");
            entity.Property(e => e.Invoicedate)
                .HasColumnType("DATE")
                .HasColumnName("INVOICEDATE");
            entity.Property(e => e.Invoiceno)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("INVOICENO");
            entity.Property(e => e.IrfcFunded)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("IRFC_FUNDED");
            entity.Property(e => e.Isgstregistered)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("ISGSTREGISTERED");
            entity.Property(e => e.Itceligible)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("ITCELIGIBLE");
            entity.Property(e => e.ItemSrno)
                .HasPrecision(4)
                .HasColumnName("ITEM_SRNO");
            entity.Property(e => e.Itemdesc)
                .IsUnicode(false)
                .HasColumnName("ITEMDESC");
            entity.Property(e => e.MaterialValue)
                .HasColumnType("NUMBER(14,3)")
                .HasColumnName("MATERIAL_VALUE");
            entity.Property(e => e.NetAmt)
                .HasColumnType("NUMBER(12,2)")
                .HasColumnName("NET_AMT");
            entity.Property(e => e.Partycode)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("PARTYCODE");
            entity.Property(e => e.Partygstin)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("PARTYGSTIN");
            entity.Property(e => e.Partyname)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("PARTYNAME");
            entity.Property(e => e.Partystate)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("PARTYSTATE");
            entity.Property(e => e.PassedAmt)
                .HasColumnType("NUMBER(12,2)")
                .HasColumnName("PASSED_AMT");
            entity.Property(e => e.PaymentDt)
                .HasColumnType("DATE")
                .HasColumnName("PAYMENT_DT");
            entity.Property(e => e.PdfLink)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("PDF_LINK");
            entity.Property(e => e.Pdffile)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("PDFFILE");
            entity.Property(e => e.PoDt)
                .HasColumnType("DATE")
                .HasColumnName("PO_DT");
            entity.Property(e => e.PoNo)
                .HasMaxLength(75)
                .IsUnicode(false)
                .HasColumnName("PO_NO");
            entity.Property(e => e.PoPdf)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("PO_PDF");
            entity.Property(e => e.Qty)
                .HasColumnType("NUMBER")
                .HasColumnName("QTY");
            entity.Property(e => e.Rate)
                .HasColumnType("NUMBER")
                .HasColumnName("RATE");
            entity.Property(e => e.RecvDate)
                .HasColumnType("DATE")
                .HasColumnName("RECV_DATE");
            entity.Property(e => e.RegionCode)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("REGION_CODE");
            entity.Property(e => e.ReturnDate)
                .HasColumnType("DATE")
                .HasColumnName("RETURN_DATE");
            entity.Property(e => e.ReturnReason)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("RETURN_REASON");
            entity.Property(e => e.Reversecharge)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("REVERSECHARGE");
            entity.Property(e => e.Rlygstin)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("RLYGSTIN");
            entity.Property(e => e.SetNo)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("SET_NO");
            entity.Property(e => e.Sgstamt)
                .HasColumnType("NUMBER")
                .HasColumnName("SGSTAMT");
            entity.Property(e => e.Sgstrate)
                .HasColumnType("NUMBER")
                .HasColumnName("SGSTRATE");
            entity.Property(e => e.Statesupply)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("STATESUPPLY");
            entity.Property(e => e.Status)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("STATUS");
            entity.Property(e => e.Ugstamt)
                .HasColumnType("NUMBER")
                .HasColumnName("UGSTAMT");
            entity.Property(e => e.Ugstrate)
                .HasColumnType("NUMBER")
                .HasColumnName("UGSTRATE");
            entity.Property(e => e.Unitcode)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("UNITCODE");
            entity.Property(e => e.UomFactor)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("UOM_FACTOR");
            entity.Property(e => e.UpdDate)
                .HasColumnType("DATE")
                .HasColumnName("UPD_DATE");
            entity.Property(e => e.Value)
                .HasColumnType("NUMBER(14,3)")
                .HasColumnName("VALUE");
            entity.Property(e => e.VendAdd1)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("VEND_ADD1");
            entity.Property(e => e.VendAdd2)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("VEND_ADD2");
            entity.Property(e => e.VendCd)
                .HasPrecision(10)
                .HasColumnName("VEND_CD");
            entity.Property(e => e.VendName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("VEND_NAME");
            entity.Property(e => e.VendorCity)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("VENDOR_CITY");
            entity.Property(e => e.Visits)
                .HasPrecision(10)
                .HasColumnName("VISITS");
        });

        modelBuilder.Entity<Rkm02>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("RKM_02");

            entity.Property(e => e.VendCd)
                .HasPrecision(6)
                .HasColumnName("VEND_CD");
        });

        modelBuilder.Entity<RkmT26ChequePosting>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("RKM_T26_CHEQUE_POSTING");

            entity.Property(e => e.BankCd)
                .HasPrecision(3)
                .HasColumnName("BANK_CD");
            entity.Property(e => e.ChqDt)
                .HasColumnType("DATE")
                .HasColumnName("CHQ_DT");
            entity.Property(e => e.ChqNo)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("CHQ_NO");
            entity.Property(e => e.ClearedAmt)
                .HasColumnType("NUMBER")
                .HasColumnName("CLEARED_AMT");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("ROLES_PK");

            entity.ToTable("ROLES");

            entity.Property(e => e.RoleId)
                .HasPrecision(6)
                .HasDefaultValueSql("\"IBSDEV\".\"ROLES_SEQ\".\"NEXTVAL\"")
                .HasColumnName("ROLE_ID");
            entity.Property(e => e.Createdby)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("CREATEDBY");
            entity.Property(e => e.Createddate)
                .HasColumnType("DATE")
                .HasColumnName("CREATEDDATE");
            entity.Property(e => e.Isactive)
                .HasPrecision(2)
                .HasColumnName("ISACTIVE");
            entity.Property(e => e.Isdeleted)
                .HasPrecision(2)
                .HasColumnName("ISDELETED");
            entity.Property(e => e.Issysadmin)
                .HasPrecision(2)
                .HasColumnName("ISSYSADMIN");
            entity.Property(e => e.Roledescription)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("ROLEDESCRIPTION");
            entity.Property(e => e.Rolename)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ROLENAME");
            entity.Property(e => e.Updatedby)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("UPDATEDBY");
            entity.Property(e => e.Updateddate)
                .HasColumnType("DATE")
                .HasColumnName("UPDATEDDATE");
        });

        modelBuilder.Entity<RptPrmIcLine>(entity =>
        {
            entity.HasKey(e => new { e.CaseNo, e.ItemSrnoPo }).HasName("PK_RPT_PARAM_ICLINE");

            entity.ToTable("RPT_PRM_IC_LINES");

            entity.Property(e => e.CaseNo)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CASE_NO");
            entity.Property(e => e.ItemSrnoPo)
                .HasPrecision(3)
                .HasColumnName("ITEM_SRNO_PO");
            entity.Property(e => e.PrevQtyOffered)
                .HasColumnType("NUMBER(11,3)")
                .HasColumnName("PREV_QTY_OFFERED");
            entity.Property(e => e.PrevQtyPassed)
                .HasColumnType("NUMBER(11,3)")
                .HasColumnName("PREV_QTY_PASSED");
            entity.Property(e => e.RequestTs)
                .HasPrecision(6)
                .HasColumnName("REQUEST_TS");
        });

        modelBuilder.Entity<RptPrmInspectionCertificate>(entity =>
        {
            entity.HasKey(e => new { e.CaseNo, e.CallRecvDt, e.CallSno, e.ConsigneeCd }).HasName("PK_RPT_PARAM_IC");

            entity.ToTable("RPT_PRM_INSPECTION_CERTIFICATE");

            entity.Property(e => e.CaseNo)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CASE_NO");
            entity.Property(e => e.CallRecvDt)
                .HasColumnType("DATE")
                .HasColumnName("CALL_RECV_DT");
            entity.Property(e => e.CallSno)
                .HasPrecision(5)
                .HasColumnName("CALL_SNO");
            entity.Property(e => e.ConsigneeCd)
                .HasPrecision(8)
                .HasColumnName("CONSIGNEE_CD");
            entity.Property(e => e.NumVisits)
                .HasPrecision(5)
                .HasColumnName("NUM_VISITS");
            entity.Property(e => e.RequestTs)
                .HasPrecision(6)
                .HasColumnName("REQUEST_TS");
            entity.Property(e => e.VisitDates)
                .HasMaxLength(500)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("VISIT_DATES");
        });

        modelBuilder.Entity<T01Region>(entity =>
        {
            entity.HasKey(e => e.RegionCode).HasName("PK_REGION_CODE");

            entity.ToTable("T01_REGIONS");

            entity.Property(e => e.RegionCode)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("REGION_CODE");
            entity.Property(e => e.Address)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("ADDRESS");
            entity.Property(e => e.BankAccNo)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("BANK_ACC_NO");
            entity.Property(e => e.BankName)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("BANK_NAME");
            entity.Property(e => e.GstinNo)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("GSTIN_NO");
            entity.Property(e => e.IfscCode)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("IFSC_CODE");
            entity.Property(e => e.PartyName)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("PARTY_NAME");
            entity.Property(e => e.Region)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("REGION");
            entity.Property(e => e.RlyPartyCd)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("RLY_PARTY_CD");
        });

        modelBuilder.Entity<T02User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK_USER_ID");

            entity.ToTable("T02_USERS");

            entity.Property(e => e.UserId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("USER_ID");
            entity.Property(e => e.AllowDnChksht)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ALLOW_DN_CHKSHT");
            entity.Property(e => e.AllowPo)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ALLOW_PO");
            entity.Property(e => e.AllowUpChksht)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ALLOW_UP_CHKSHT");
            entity.Property(e => e.AuthLevl)
                .HasPrecision(2)
                .HasColumnName("AUTH_LEVL");
            entity.Property(e => e.CallMarking)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CALL_MARKING");
            entity.Property(e => e.CallRemarking)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CALL_REMARKING");
            entity.Property(e => e.Createdby)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CREATEDBY");
            entity.Property(e => e.Createddate)
                .HasColumnType("TIMESTAMP(6) WITH TIME ZONE")
                .HasColumnName("CREATEDDATE");
            entity.Property(e => e.EmpNo)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("EMP_NO");
            entity.Property(e => e.Id)
                .HasDefaultValueSql("\"IBSDEV\".\"T02_USERS_SEQ\".\"NEXTVAL\"")
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ID");
            entity.Property(e => e.Isdeleted)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ISDELETED");
            entity.Property(e => e.Password)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("PASSWORD");
            entity.Property(e => e.Region)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("REGION");
            entity.Property(e => e.RitesEmp)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("RITES_EMP");
            entity.Property(e => e.Status)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("STATUS");
            entity.Property(e => e.Updatedby)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("UPDATEDBY");
            entity.Property(e => e.Updateddate)
                .HasColumnType("TIMESTAMP(6) WITH TIME ZONE")
                .HasColumnName("UPDATEDDATE");
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("USER_NAME");
            entity.Property(e => e.UserType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("USER_TYPE");
        });

        modelBuilder.Entity<T03City>(entity =>
        {
            entity.HasKey(e => e.CityCd).HasName("PK_CITY");

            entity.ToTable("T03_CITY");

            entity.Property(e => e.CityCd)
                .HasPrecision(6)
                .ValueGeneratedNever()
                .HasColumnName("CITY_CD");
            entity.Property(e => e.City)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CITY");
            entity.Property(e => e.Createdby)
                .HasPrecision(6)
                .HasColumnName("CREATEDBY");
            entity.Property(e => e.Createddate)
                .HasColumnType("DATE")
                .HasColumnName("CREATEDDATE");
            entity.Property(e => e.Datetime)
                .HasColumnType("DATE")
                .HasColumnName("DATETIME");
            entity.Property(e => e.Isdeleted)
                .HasPrecision(2)
                .HasColumnName("ISDELETED");
            entity.Property(e => e.Location)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("LOCATION");
            entity.Property(e => e.PinCode)
                .HasMaxLength(6)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("PIN_CODE");
            entity.Property(e => e.StateCd)
                .HasPrecision(2)
                .HasColumnName("STATE_CD");
            entity.Property(e => e.Updatedby)
                .HasPrecision(6)
                .HasColumnName("UPDATEDBY");
            entity.Property(e => e.Updateddate)
                .HasColumnType("DATE")
                .HasColumnName("UPDATEDDATE");
            entity.Property(e => e.UserId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("USER_ID");

            entity.HasOne(d => d.StateCdNavigation).WithMany(p => p.T03Cities)
                .HasForeignKey(d => d.StateCd)
                .HasConstraintName("FK_STATE_CD");
        });

        modelBuilder.Entity<T04Uom>(entity =>
        {
            entity.HasKey(e => e.UomCd).HasName("PK_UOM_CD");

            entity.ToTable("T04_UOM");

            entity.Property(e => e.UomCd)
                .HasPrecision(3)
                .HasDefaultValueSql("\"IBSDEV\".\"T04_UOM_SEQ\".\"NEXTVAL\"")
                .HasColumnName("UOM_CD");
            entity.Property(e => e.Createdby)
                .HasPrecision(6)
                .HasColumnName("CREATEDBY");
            entity.Property(e => e.Createddate)
                .HasColumnType("DATE")
                .HasColumnName("CREATEDDATE");
            entity.Property(e => e.Datetime)
                .HasColumnType("DATE")
                .HasColumnName("DATETIME");
            entity.Property(e => e.ImmsUomCd)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("IMMS_UOM_CD");
            entity.Property(e => e.Isdeleted)
                .HasPrecision(2)
                .HasColumnName("ISDELETED");
            entity.Property(e => e.UomFactor)
                .HasColumnType("NUMBER(7,2)")
                .HasColumnName("UOM_FACTOR");
            entity.Property(e => e.UomLDesc)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("UOM_L_DESC");
            entity.Property(e => e.UomSDesc)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("UOM_S_DESC");
            entity.Property(e => e.Updatedby)
                .HasPrecision(6)
                .HasColumnName("UPDATEDBY");
            entity.Property(e => e.Updateddate)
                .HasColumnType("DATE")
                .HasColumnName("UPDATEDDATE");
            entity.Property(e => e.UserId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("USER_ID");
        });

        modelBuilder.Entity<T05Vendor>(entity =>
        {
            entity.HasKey(e => e.VendCd).HasName("PK_VENDOR_CD");

            entity.ToTable("T05_VENDOR");

            entity.Property(e => e.VendCd)
                .HasPrecision(6)
                .ValueGeneratedNever()
                .HasColumnName("VEND_CD");
            entity.Property(e => e.Datetime)
                .HasColumnType("DATE")
                .HasColumnName("DATETIME");
            entity.Property(e => e.Id)
                .HasDefaultValueSql("\"IBSDEV\".\"T05_VENDOR_SEQ\".\"NEXTVAL\"")
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ID");
            entity.Property(e => e.OnlineCallStatus)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ONLINE_CALL_STATUS");
            entity.Property(e => e.UserId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("USER_ID");
            entity.Property(e => e.VendAdd1)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("VEND_ADD1");
            entity.Property(e => e.VendAdd2)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("VEND_ADD2");
            entity.Property(e => e.VendApproval)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("VEND_APPROVAL");
            entity.Property(e => e.VendApprovalFr)
                .HasColumnType("DATE")
                .HasColumnName("VEND_APPROVAL_FR");
            entity.Property(e => e.VendApprovalTo)
                .HasColumnType("DATE")
                .HasColumnName("VEND_APPROVAL_TO");
            entity.Property(e => e.VendCdAlpha)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("VEND_CD_ALPHA");
            entity.Property(e => e.VendCityCd)
                .HasPrecision(6)
                .HasColumnName("VEND_CITY_CD");
            entity.Property(e => e.VendContactPer1)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("VEND_CONTACT_PER_1");
            entity.Property(e => e.VendContactPer2)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("VEND_CONTACT_PER_2");
            entity.Property(e => e.VendContactTel1)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("VEND_CONTACT_TEL_1");
            entity.Property(e => e.VendContactTel2)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("VEND_CONTACT_TEL_2");
            entity.Property(e => e.VendEmail)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("VEND_EMAIL");
            entity.Property(e => e.VendInspStopped)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("VEND_INSP_STOPPED");
            entity.Property(e => e.VendName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("VEND_NAME");
            entity.Property(e => e.VendPwd)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("VEND_PWD");
            entity.Property(e => e.VendRemarks)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .HasColumnName("VEND_REMARKS");
            entity.Property(e => e.VendStatus)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("VEND_STATUS");
            entity.Property(e => e.VendStatusDtFr)
                .HasColumnType("DATE")
                .HasColumnName("VEND_STATUS_DT_FR");
            entity.Property(e => e.VendStatusDtTo)
                .HasColumnType("DATE")
                .HasColumnName("VEND_STATUS_DT_TO");

            entity.HasOne(d => d.VendCityCdNavigation).WithMany(p => p.T05Vendors)
                .HasForeignKey(d => d.VendCityCd)
                .HasConstraintName("FK_VEND_CITY_CD");
        });

        modelBuilder.Entity<T06Code>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("T06_CODE");

            entity.Property(e => e.PurchaserCd)
                .HasPrecision(8)
                .HasColumnName("PURCHASER_CD");
        });

        modelBuilder.Entity<T06Consignee>(entity =>
        {
            entity.HasKey(e => e.ConsigneeCd).HasName("PK_CONSIGNEE_CD");

            entity.ToTable("T06_CONSIGNEE");

            entity.Property(e => e.ConsigneeCd)
                .HasPrecision(8)
                .ValueGeneratedNever()
                .HasColumnName("CONSIGNEE_CD");
            entity.Property(e => e.ConsigneeAdd1)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("CONSIGNEE_ADD1");
            entity.Property(e => e.ConsigneeAdd2)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("CONSIGNEE_ADD2");
            entity.Property(e => e.ConsigneeCity)
                .HasPrecision(6)
                .HasColumnName("CONSIGNEE_CITY");
            entity.Property(e => e.ConsigneeDept)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("CONSIGNEE_DEPT");
            entity.Property(e => e.ConsigneeDesig)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("CONSIGNEE_DESIG");
            entity.Property(e => e.ConsigneeFirm)
                .HasMaxLength(75)
                .IsUnicode(false)
                .HasColumnName("CONSIGNEE_FIRM");
            entity.Property(e => e.ConsigneeType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CONSIGNEE_TYPE");
            entity.Property(e => e.Createdby)
                .HasPrecision(6)
                .HasColumnName("CREATEDBY");
            entity.Property(e => e.Createddate)
                .HasColumnType("DATE")
                .HasColumnName("CREATEDDATE");
            entity.Property(e => e.Datetime)
                .HasColumnType("DATE")
                .HasColumnName("DATETIME");
            entity.Property(e => e.GstinNo)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("GSTIN_NO");
            entity.Property(e => e.Isdeleted)
                .HasPrecision(2)
                .HasColumnName("ISDELETED");
            entity.Property(e => e.LegalName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("LEGAL_NAME");
            entity.Property(e => e.PinCode)
                .HasMaxLength(6)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("PIN_CODE");
            entity.Property(e => e.SapCustCdCon)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("SAP_CUST_CD_CON");
            entity.Property(e => e.Updatedby)
                .HasPrecision(6)
                .HasColumnName("UPDATEDBY");
            entity.Property(e => e.Updateddate)
                .HasColumnType("DATE")
                .HasColumnName("UPDATEDDATE");
            entity.Property(e => e.UserId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("USER_ID");

            entity.HasOne(d => d.ConsigneeCityNavigation).WithMany(p => p.T06Consignees)
                .HasForeignKey(d => d.ConsigneeCity)
                .HasConstraintName("FK_CONSIGNEE_CITY");
        });

        modelBuilder.Entity<T07RitesDesig>(entity =>
        {
            entity.HasKey(e => e.RDesigCd).HasName("PK_R_DESIG_CD");

            entity.ToTable("T07_RITES_DESIG");

            entity.Property(e => e.RDesigCd)
                .HasPrecision(3)
                .HasDefaultValueSql("\"IBSDEV\".\"T07_RITES_DESIG_SEQ\".\"NEXTVAL\"")
                .HasColumnName("R_DESIG_CD");
            entity.Property(e => e.Createdby)
                .HasPrecision(6)
                .HasColumnName("CREATEDBY");
            entity.Property(e => e.Createddate)
                .HasColumnType("DATE")
                .HasColumnName("CREATEDDATE");
            entity.Property(e => e.Isdeleted)
                .HasPrecision(2)
                .HasColumnName("ISDELETED");
            entity.Property(e => e.RDesignation)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("R_DESIGNATION");
            entity.Property(e => e.Updatedby)
                .HasPrecision(6)
                .HasColumnName("UPDATEDBY");
            entity.Property(e => e.Updateddate)
                .HasColumnType("DATE")
                .HasColumnName("UPDATEDDATE");
        });

        modelBuilder.Entity<T08IeControllOfficer>(entity =>
        {
            entity.HasKey(e => e.CoCd).HasName("PK_CO_CD");

            entity.ToTable("T08_IE_CONTROLL_OFFICER");

            entity.Property(e => e.CoCd)
                .HasPrecision(3)
                .HasColumnName("CO_CD");
            entity.Property(e => e.CoDesig)
                .HasPrecision(2)
                .HasColumnName("CO_DESIG");
            entity.Property(e => e.CoEmail)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CO_EMAIL");
            entity.Property(e => e.CoName)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("CO_NAME");
            entity.Property(e => e.CoPhoneNo)
                .HasMaxLength(21)
                .IsUnicode(false)
                .HasColumnName("CO_PHONE_NO");
            entity.Property(e => e.CoRegion)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CO_REGION");
            entity.Property(e => e.CoStatus)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CO_STATUS");
            entity.Property(e => e.CoStatusDt)
                .HasColumnType("DATE")
                .HasColumnName("CO_STATUS_DT");
            entity.Property(e => e.CoType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CO_TYPE");

            entity.HasOne(d => d.CoDesigNavigation).WithMany(p => p.T08IeControllOfficers)
                .HasForeignKey(d => d.CoDesig)
                .HasConstraintName("FK_CO_DESIG");

            entity.HasOne(d => d.CoRegionNavigation).WithMany(p => p.T08IeControllOfficers)
                .HasForeignKey(d => d.CoRegion)
                .HasConstraintName("FK_CO_REGION");
        });

        modelBuilder.Entity<T09Ie>(entity =>
        {
            entity.HasKey(e => e.IeCd).HasName("PK_IE_CD");

            entity.ToTable("T09_IE");

            entity.Property(e => e.IeCd)
                .HasPrecision(6)
                .ValueGeneratedNever()
                .HasColumnName("IE_CD");
            entity.Property(e => e.AltIe)
                .HasPrecision(6)
                .HasColumnName("ALT_IE");
            entity.Property(e => e.AltIeThree)
                .HasPrecision(6)
                .HasColumnName("ALT_IE_THREE");
            entity.Property(e => e.AltIeTwo)
                .HasPrecision(6)
                .HasColumnName("ALT_IE_TWO");
            entity.Property(e => e.CallMarkingStoppingDt)
                .HasColumnType("DATE")
                .HasColumnName("CALL_MARKING_STOPPING_DT");
            entity.Property(e => e.Datetime)
                .HasColumnType("DATE")
                .HasColumnName("DATETIME");
            entity.Property(e => e.DscExpiryDt)
                .HasColumnType("DATE")
                .HasColumnName("DSC_EXPIRY_DT");
            entity.Property(e => e.IeCallMarking)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("IE_CALL_MARKING");
            entity.Property(e => e.IeCityCd)
                .HasPrecision(4)
                .HasColumnName("IE_CITY_CD");
            entity.Property(e => e.IeCoCd)
                .HasPrecision(3)
                .HasColumnName("IE_CO_CD");
            entity.Property(e => e.IeDepartment)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("IE_DEPARTMENT");
            entity.Property(e => e.IeDesig)
                .HasPrecision(2)
                .HasColumnName("IE_DESIG");
            entity.Property(e => e.IeDob)
                .HasColumnType("DATE")
                .HasColumnName("IE_DOB");
            entity.Property(e => e.IeEmail)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("IE_EMAIL");
            entity.Property(e => e.IeEmpNo)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("IE_EMP_NO");
            entity.Property(e => e.IeJoinDt)
                .HasColumnType("DATE")
                .HasColumnName("IE_JOIN_DT");
            entity.Property(e => e.IeName)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("IE_NAME");
            entity.Property(e => e.IePhoneNo)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("IE_PHONE_NO");
            entity.Property(e => e.IePwd)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("IE_PWD");
            entity.Property(e => e.IeRegion)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("IE_REGION");
            entity.Property(e => e.IeSealNo)
                .HasPrecision(4)
                .HasColumnName("IE_SEAL_NO");
            entity.Property(e => e.IeSname)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("IE_SNAME");
            entity.Property(e => e.IeStatus)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("IE_STATUS");
            entity.Property(e => e.IeStatusDt)
                .HasColumnType("DATE")
                .HasColumnName("IE_STATUS_DT");
            entity.Property(e => e.IeType)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("IE_TYPE");
            entity.Property(e => e.UserId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("USER_ID");

            entity.HasOne(d => d.IeCoCdNavigation).WithMany(p => p.T09Ies)
                .HasForeignKey(d => d.IeCoCd)
                .HasConstraintName("FK_IE_CO");

            entity.HasOne(d => d.IeDesigNavigation).WithMany(p => p.T09Ies)
                .HasForeignKey(d => d.IeDesig)
                .HasConstraintName("FK_IE_DESIG");

            entity.HasOne(d => d.IeRegionNavigation).WithMany(p => p.T09Ies)
                .HasForeignKey(d => d.IeRegion)
                .HasConstraintName("FK_IE_REGION");
        });

        modelBuilder.Entity<T100VenderCluster>(entity =>
        {
            entity.HasKey(e => new { e.VendorCode, e.DepartmentName }).HasName("PK_VENDER_CODE");

            entity.ToTable("T100_VENDER_CLUSTER");

            entity.Property(e => e.VendorCode)
                .HasPrecision(6)
                .HasColumnName("VENDOR_CODE");
            entity.Property(e => e.DepartmentName)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("DEPARTMENT_NAME");
            entity.Property(e => e.ClusterCode)
                .HasPrecision(3)
                .HasColumnName("CLUSTER_CODE");
            entity.Property(e => e.Datetime)
                .HasColumnType("DATE")
                .HasColumnName("DATETIME");
            entity.Property(e => e.UserId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("USER_ID");
        });

        modelBuilder.Entity<T101IeCluster>(entity =>
        {
            entity.HasKey(e => new { e.ClusterCode, e.DepartmentCode });

            entity.ToTable("T101_IE_CLUSTER");

            entity.Property(e => e.ClusterCode)
                .HasPrecision(3)
                .HasColumnName("CLUSTER_CODE");
            entity.Property(e => e.DepartmentCode)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("DEPARTMENT_CODE");
            entity.Property(e => e.Datetime)
                .HasColumnType("DATE")
                .HasColumnName("DATETIME");
            entity.Property(e => e.IeCode)
                .HasPrecision(6)
                .HasColumnName("IE_CODE");
            entity.Property(e => e.UserId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("USER_ID");
        });

        modelBuilder.Entity<T102IeMaximumCallLimit>(entity =>
        {
            entity.HasKey(e => e.RegionCode);

            entity.ToTable("T102_IE_MAXIMUM_CALL_LIMIT");

            entity.Property(e => e.RegionCode)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("REGION_CODE");
            entity.Property(e => e.Datetime)
                .HasColumnType("DATE")
                .HasColumnName("DATETIME");
            entity.Property(e => e.MaximumCall)
                .HasPrecision(3)
                .HasColumnName("MAXIMUM_CALL");
            entity.Property(e => e.UserId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("USER_ID");
        });

        modelBuilder.Entity<T103VendDoc>(entity =>
        {
            entity.HasKey(e => new { e.VendCd, e.DocType }).HasName("T103_VEND_DOCS");

            entity.ToTable("T103_VEND_DOCS");

            entity.Property(e => e.VendCd)
                .HasPrecision(6)
                .HasColumnName("VEND_CD");
            entity.Property(e => e.DocType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("DOC_TYPE");
            entity.Property(e => e.Datetime)
                .HasColumnType("DATE")
                .HasColumnName("DATETIME");
        });

        modelBuilder.Entity<T104VendEquipClbrCert>(entity =>
        {
            entity.HasKey(e => new { e.VendCd, e.DocType, e.EquipMkSl, e.CalibCertNo });

            entity.ToTable("T104_VEND_EQUIP_CLBR_CERT");

            entity.Property(e => e.VendCd)
                .HasPrecision(6)
                .HasColumnName("VEND_CD");
            entity.Property(e => e.DocType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("DOC_TYPE");
            entity.Property(e => e.EquipMkSl)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("EQUIP_MK_SL");
            entity.Property(e => e.CalibCertNo)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("CALIB_CERT_NO");
            entity.Property(e => e.CalibratedBy)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("CALIBRATED_BY");
            entity.Property(e => e.Datetime)
                .HasColumnType("DATE")
                .HasColumnName("DATETIME");
            entity.Property(e => e.DtOfCalib)
                .HasColumnType("DATE")
                .HasColumnName("DT_OF_CALIB");
            entity.Property(e => e.EquipClbrCertSno)
                .HasPrecision(3)
                .HasColumnName("EQUIP_CLBR_CERT_SNO");
            entity.Property(e => e.EquipDesc)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("EQUIP_DESC");
            entity.Property(e => e.EquipName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("EQUIP_NAME");
            entity.Property(e => e.EquipRange)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("EQUIP_RANGE");
            entity.Property(e => e.NablAccDet)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("NABL_ACC_DET");
            entity.Property(e => e.NextDtCalib)
                .HasColumnType("DATE")
                .HasColumnName("NEXT_DT_CALIB");

            entity.HasOne(d => d.T103VendDoc).WithMany(p => p.T104VendEquipClbrCerts)
                .HasForeignKey(d => new { d.VendCd, d.DocType })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_T104_VEND_EQUIP_CLBR_CERT");
        });

        modelBuilder.Entity<T105LoLogin>(entity =>
        {
            entity.HasKey(e => e.Mobile).HasName("PK105_LO_ID");

            entity.ToTable("T105_LO_LOGIN");

            entity.Property(e => e.Mobile)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MOBILE");
            entity.Property(e => e.Designation)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("DESIGNATION");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.LoName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("LO_NAME");
            entity.Property(e => e.LoPerFr)
                .HasColumnType("DATE")
                .HasColumnName("LO_PER_FR");
            entity.Property(e => e.LoPerTo)
                .HasColumnType("DATE")
                .HasColumnName("LO_PER_TO");
            entity.Property(e => e.Pwd)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("PWD");
            entity.Property(e => e.Status)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("STATUS");
        });

        modelBuilder.Entity<T106LoOrgn>(entity =>
        {
            entity.HasKey(e => new { e.Mobile, e.OrgnType, e.OrgnChased }).HasName("PK106_LO_ORGN");

            entity.ToTable("T106_LO_ORGN");

            entity.Property(e => e.Mobile)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MOBILE");
            entity.Property(e => e.OrgnType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ORGN_TYPE");
            entity.Property(e => e.OrgnChased)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("ORGN_CHASED");
        });

        modelBuilder.Entity<T107LoLogginLog>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("T107_LO_LOGGIN_LOG");

            entity.Property(e => e.LogginTime)
                .HasColumnType("DATE")
                .HasColumnName("LOGGIN_TIME");
            entity.Property(e => e.Mobile)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MOBILE");
            entity.Property(e => e.Otp)
                .HasPrecision(4)
                .HasColumnName("OTP");
            entity.Property(e => e.OtpExpTime)
                .HasColumnType("DATE")
                .HasColumnName("OTP_EXP_TIME");
            entity.Property(e => e.OtpGenTime)
                .HasColumnType("DATE")
                .HasColumnName("OTP_GEN_TIME");
            entity.Property(e => e.Status)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("STATUS");
        });

        modelBuilder.Entity<T108RemarkedCall>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("T108_REMARKED_CALLS");

            entity.Property(e => e.CallRecvDt)
                .HasColumnType("DATE")
                .HasColumnName("CALL_RECV_DT");
            entity.Property(e => e.CallRemarkStatus)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CALL_REMARK_STATUS");
            entity.Property(e => e.CallSno)
                .HasPrecision(5)
                .HasColumnName("CALL_SNO");
            entity.Property(e => e.CaseNo)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CASE_NO");
            entity.Property(e => e.FrIeCd)
                .HasPrecision(4)
                .HasColumnName("FR_IE_CD");
            entity.Property(e => e.FrIePendingCalls)
                .HasPrecision(2)
                .HasColumnName("FR_IE_PENDING_CALLS");
            entity.Property(e => e.RemAppBy)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("REM_APP_BY");
            entity.Property(e => e.RemAppDatetime)
                .HasColumnType("DATE")
                .HasColumnName("REM_APP_DATETIME");
            entity.Property(e => e.RemInitBy)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("REM_INIT_BY");
            entity.Property(e => e.RemInitDatetime)
                .HasColumnType("DATE")
                .HasColumnName("REM_INIT_DATETIME");
            entity.Property(e => e.RemRejRemark)
                .HasMaxLength(400)
                .IsUnicode(false)
                .HasColumnName("REM_REJ_REMARK");
            entity.Property(e => e.RemarkReason)
                .HasMaxLength(400)
                .IsUnicode(false)
                .HasColumnName("REMARK_REASON");
            entity.Property(e => e.RemarkingStatus)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("REMARKING_STATUS");
            entity.Property(e => e.ToIeCd)
                .HasPrecision(4)
                .HasColumnName("TO_IE_CD");
            entity.Property(e => e.ToIePendingCalls)
                .HasPrecision(2)
                .HasColumnName("TO_IE_PENDING_CALLS");
        });

        modelBuilder.Entity<T109LabSampleInfo>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("T109_LAB_SAMPLE_INFO");

            entity.Property(e => e.CallRecvDt)
                .HasColumnType("DATE")
                .HasColumnName("CALL_RECV_DT");
            entity.Property(e => e.CallSno)
                .HasPrecision(5)
                .HasColumnName("CALL_SNO");
            entity.Property(e => e.CaseNo)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CASE_NO");
            entity.Property(e => e.Datetime)
                .HasColumnType("DATE")
                .HasColumnName("DATETIME");
            entity.Property(e => e.DepositSlipUploaded)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("DEPOSIT_SLIP_UPLOADED");
            entity.Property(e => e.FeeDepositConfirm)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("FEE_DEPOSIT_CONFIRM");
            entity.Property(e => e.IeCd)
                .HasPrecision(4)
                .HasColumnName("IE_CD");
            entity.Property(e => e.LabRepUploadedDt)
                .HasColumnType("DATE")
                .HasColumnName("LAB_REP_UPLOADED_DT");
            entity.Property(e => e.LikelyDtReport)
                .HasColumnType("DATE")
                .HasColumnName("LIKELY_DT_REPORT");
            entity.Property(e => e.Remarks)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("REMARKS");
            entity.Property(e => e.SampleRecvDt)
                .HasColumnType("DATE")
                .HasColumnName("SAMPLE_RECV_DT");
            entity.Property(e => e.Status)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("STATUS");
            entity.Property(e => e.TestingCharges)
                .HasPrecision(11)
                .HasColumnName("TESTING_CHARGES");
            entity.Property(e => e.UserId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("USER_ID");

            entity.HasOne(d => d.Ca).WithMany()
                .HasForeignKey(d => new { d.CaseNo, d.CallRecvDt, d.CallSno })
                .HasConstraintName("FK_109_LAB_SAMPLE_INFO");
        });

        modelBuilder.Entity<T10IcBookset>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("T10_IC_BOOKSET");

            entity.Property(e => e.BkNo)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("BK_NO");
            entity.Property(e => e.BkSubmitDt)
                .HasColumnType("DATE")
                .HasColumnName("BK_SUBMIT_DT");
            entity.Property(e => e.BkSubmitted)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("BK_SUBMITTED");
            entity.Property(e => e.CutOffDt)
                .HasColumnType("DATE")
                .HasColumnName("CUT_OFF_DT");
            entity.Property(e => e.CutOffSet)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("CUT_OFF_SET");
            entity.Property(e => e.Datetime)
                .HasColumnType("DATE")
                .HasColumnName("DATETIME");
            entity.Property(e => e.IssueDt)
                .HasColumnType("DATE")
                .HasColumnName("ISSUE_DT");
            entity.Property(e => e.IssueToIecd)
                .HasPrecision(6)
                .HasColumnName("ISSUE_TO_IECD");
            entity.Property(e => e.Region)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("REGION");
            entity.Property(e => e.SetNoFr)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("SET_NO_FR");
            entity.Property(e => e.SetNoTo)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("SET_NO_TO");
            entity.Property(e => e.UserId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("USER_ID");

            entity.HasOne(d => d.IssueToIecdNavigation).WithMany()
                .HasForeignKey(d => d.IssueToIecd)
                .HasConstraintName("FK_IECD");
        });

        modelBuilder.Entity<T110LabDoc>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("T110_LAB_DOC");

            entity.Property(e => e.CallRecvDt)
                .HasColumnType("DATE")
                .HasColumnName("CALL_RECV_DT");
            entity.Property(e => e.CallSno)
                .HasPrecision(5)
                .HasColumnName("CALL_SNO");
            entity.Property(e => e.CaseNo)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CASE_NO");
            entity.Property(e => e.DocAppBy)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("DOC_APP_BY");
            entity.Property(e => e.DocAppDatetime)
                .HasColumnType("DATE")
                .HasColumnName("DOC_APP_DATETIME");
            entity.Property(e => e.DocInitDatetime)
                .HasColumnType("DATE")
                .HasColumnName("DOC_INIT_DATETIME");
            entity.Property(e => e.DocRejRemark)
                .HasMaxLength(400)
                .IsUnicode(false)
                .HasColumnName("DOC_REJ_REMARK");
            entity.Property(e => e.DocStatusFin)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("DOC_STATUS_FIN");
            entity.Property(e => e.DocStatusVend)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("DOC_STATUS_VEND");
            entity.Property(e => e.DocUpdBy)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("DOC_UPD_BY");
            entity.Property(e => e.Tds)
                .HasPrecision(9)
                .HasColumnName("TDS");
            entity.Property(e => e.TestingCharges)
                .HasPrecision(11)
                .HasColumnName("TESTING_CHARGES");
            entity.Property(e => e.UtrDt)
                .HasColumnType("DATE")
                .HasColumnName("UTR_DT");
            entity.Property(e => e.UtrNo)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("UTR_NO");

            entity.HasOne(d => d.Ca).WithMany()
                .HasForeignKey(d => new { d.CaseNo, d.CallRecvDt, d.CallSno })
                .HasConstraintName("FK_110_LAB_DOC");
        });

        modelBuilder.Entity<T11CallCancelCode>(entity =>
        {
            entity.HasKey(e => e.CancelCd).HasName("PK_CANCEL_CD");

            entity.ToTable("T11_CALL_CANCEL_CODES");

            entity.Property(e => e.CancelCd)
                .HasPrecision(2)
                .HasColumnName("CANCEL_CD");
            entity.Property(e => e.CancelDesc)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("CANCEL_DESC");
        });

        modelBuilder.Entity<T12BillPayingOfficer>(entity =>
        {
            entity.HasKey(e => e.BpoCd).HasName("PK_BPO_CD");

            entity.ToTable("T12_BILL_PAYING_OFFICER");

            entity.Property(e => e.BpoCd)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("BPO_CD");
            entity.Property(e => e.Au)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("AU");
            entity.Property(e => e.BillPassOfficer)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("BILL_PASS_OFFICER");
            entity.Property(e => e.BpoAdd)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("BPO_ADD");
            entity.Property(e => e.BpoAdd1)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("BPO_ADD1");
            entity.Property(e => e.BpoAdd2)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("BPO_ADD2");
            entity.Property(e => e.BpoAdvFlg)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("BPO_ADV_FLG");
            entity.Property(e => e.BpoCdOld)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("BPO_CD_OLD");
            entity.Property(e => e.BpoCityCd)
                .HasPrecision(4)
                .HasColumnName("BPO_CITY_CD");
            entity.Property(e => e.BpoEmail)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("BPO_EMAIL");
            entity.Property(e => e.BpoFax)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("BPO_FAX");
            entity.Property(e => e.BpoFee)
                .HasColumnType("NUMBER(11,4)")
                .HasColumnName("BPO_FEE");
            entity.Property(e => e.BpoFeeType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("BPO_FEE_TYPE");
            entity.Property(e => e.BpoFlg)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("BPO_FLG");
            entity.Property(e => e.BpoLocCd)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("BPO_LOC_CD");
            entity.Property(e => e.BpoName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("BPO_NAME");
            entity.Property(e => e.BpoOrgn)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("BPO_ORGN");
            entity.Property(e => e.BpoPhone)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("BPO_PHONE");
            entity.Property(e => e.BpoPin)
                .HasMaxLength(7)
                .IsUnicode(false)
                .HasColumnName("BPO_PIN");
            entity.Property(e => e.BpoRegion)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("BPO_REGION");
            entity.Property(e => e.BpoRly)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("BPO_RLY");
            entity.Property(e => e.BpoState)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("BPO_STATE");
            entity.Property(e => e.BpoTaxType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("BPO_TAX_TYPE");
            entity.Property(e => e.BpoType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("BPO_TYPE");
            entity.Property(e => e.Datetime)
                .HasColumnType("DATE")
                .HasColumnName("DATETIME");
            entity.Property(e => e.GstinNo)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("GSTIN_NO");
            entity.Property(e => e.LegalName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("LEGAL_NAME");
            entity.Property(e => e.PayWindowId)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("PAY_WINDOW_ID");
            entity.Property(e => e.PinCode)
                .HasMaxLength(6)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("PIN_CODE");
            entity.Property(e => e.SapCustCdBpo)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("SAP_CUST_CD_BPO");
            entity.Property(e => e.UserId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("USER_ID");

            entity.HasOne(d => d.BpoRegionNavigation).WithMany(p => p.T12BillPayingOfficers)
                .HasForeignKey(d => d.BpoRegion)
                .HasConstraintName("FK_BPO_REGION");
        });

        modelBuilder.Entity<T13PoMaster>(entity =>
        {
            entity.HasKey(e => e.CaseNo).HasName("PK_PO_NO");

            entity.ToTable("T13_PO_MASTER");

            entity.Property(e => e.CaseNo)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CASE_NO");
            entity.Property(e => e.Createdby)
                .HasPrecision(6)
                .HasColumnName("CREATEDBY");
            entity.Property(e => e.Createddate)
                .HasColumnType("DATE")
                .HasColumnName("CREATEDDATE");
            entity.Property(e => e.Datetime)
                .HasColumnType("DATE")
                .HasColumnName("DATETIME");
            entity.Property(e => e.Id)
                .HasPrecision(6)
                .HasDefaultValueSql("\"IBSDEV\".\"T13_PO_MASTER_SEQ\".\"NEXTVAL\"")
                .HasColumnName("ID");
            entity.Property(e => e.InspectingAgency)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("INSPECTING_AGENCY");
            entity.Property(e => e.Isdeleted)
                .HasPrecision(2)
                .HasDefaultValueSql("0")
                .HasColumnName("ISDELETED");
            entity.Property(e => e.L5noPo)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("L5NO_PO");
            entity.Property(e => e.PendingCharges)
                .HasPrecision(2)
                .HasColumnName("PENDING_CHARGES");
            entity.Property(e => e.PoDt)
                .HasColumnType("DATE")
                .HasColumnName("PO_DT");
            entity.Property(e => e.PoNo)
                .HasMaxLength(75)
                .IsUnicode(false)
                .HasColumnName("PO_NO");
            entity.Property(e => e.PoOrLetter)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("PO_OR_LETTER");
            entity.Property(e => e.PoSource)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("PO_SOURCE");
            entity.Property(e => e.PoiCd)
                .HasPrecision(6)
                .HasColumnName("POI_CD");
            entity.Property(e => e.PurchaserCd)
                .HasPrecision(8)
                .HasColumnName("PURCHASER_CD");
            entity.Property(e => e.RecvDt)
                .HasColumnType("DATE")
                .HasColumnName("RECV_DT");
            entity.Property(e => e.RegionCode)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("REGION_CODE");
            entity.Property(e => e.Remarks)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("REMARKS");
            entity.Property(e => e.RlyCd)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("RLY_CD");
            entity.Property(e => e.RlyNonrly)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("RLY_NONRLY");
            entity.Property(e => e.StockNonstock)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("STOCK_NONSTOCK");
            entity.Property(e => e.Updatedby)
                .HasPrecision(6)
                .HasColumnName("UPDATEDBY");
            entity.Property(e => e.Updateddate)
                .HasColumnType("DATE")
                .HasColumnName("UPDATEDDATE");
            entity.Property(e => e.UserId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("USER_ID");
            entity.Property(e => e.VendCd)
                .HasPrecision(6)
                .HasColumnName("VEND_CD");

            entity.HasOne(d => d.PurchaserCdNavigation).WithMany(p => p.T13PoMasters)
                .HasForeignKey(d => d.PurchaserCd)
                .HasConstraintName("FK_PURCHASER_CD");

            entity.HasOne(d => d.RegionCodeNavigation).WithMany(p => p.T13PoMasters)
                .HasForeignKey(d => d.RegionCode)
                .HasConstraintName("FK_REGION_CODE");

            entity.HasOne(d => d.VendCdNavigation).WithMany(p => p.T13PoMasters)
                .HasForeignKey(d => d.VendCd)
                .HasConstraintName("FK_VEND_CD");
        });

        modelBuilder.Entity<T13T15View>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("T13_T15_VIEW");

            entity.Property(e => e.Caseid)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CASEID");
            entity.Property(e => e.RecvDt)
                .HasColumnType("DATE")
                .HasColumnName("RECV_DT");
            entity.Property(e => e.Value)
                .HasColumnType("NUMBER")
                .HasColumnName("VALUE");
        });

        modelBuilder.Entity<T14PoBpo>(entity =>
        {
            entity.HasKey(e => new { e.CaseNo, e.ConsigneeCd }).HasName("PK_14_PO_BPO");

            entity.ToTable("T14_PO_BPO");

            entity.Property(e => e.CaseNo)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CASE_NO");
            entity.Property(e => e.ConsigneeCd)
                .HasPrecision(8)
                .HasColumnName("CONSIGNEE_CD");
            entity.Property(e => e.BpoCd)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("BPO_CD");

            entity.HasOne(d => d.BpoCdNavigation).WithMany(p => p.T14PoBpos)
                .HasForeignKey(d => d.BpoCd)
                .HasConstraintName("FK14_BPO_CD");

            entity.HasOne(d => d.CaseNoNavigation).WithMany(p => p.T14PoBpos)
                .HasForeignKey(d => d.CaseNo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK14_CASE_NO");

            entity.HasOne(d => d.ConsigneeCdNavigation).WithMany(p => p.T14PoBpos)
                .HasForeignKey(d => d.ConsigneeCd)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK14_CONSIGNEE_CD");
        });

        modelBuilder.Entity<T14aPoNonrly>(entity =>
        {
            entity.HasKey(e => e.CaseNo).HasName("PK_14A_CASE_NO");

            entity.ToTable("T14A_PO_NONRLY");

            entity.Property(e => e.CaseNo)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CASE_NO");
            entity.Property(e => e.ContractDt)
                .HasColumnType("DATE")
                .HasColumnName("CONTRACT_DT");
            entity.Property(e => e.ContractNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CONTRACT_NO");
            entity.Property(e => e.MaxFee)
                .HasPrecision(11)
                .HasColumnName("MAX_FEE");
            entity.Property(e => e.MinFee)
                .HasPrecision(11)
                .HasColumnName("MIN_FEE");
            entity.Property(e => e.ProjectRef)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("PROJECT_REF");
            entity.Property(e => e.WithServTax)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("WITH_SERV_TAX");

            entity.HasOne(d => d.CaseNoNavigation).WithOne(p => p.T14aPoNonrly)
                .HasForeignKey<T14aPoNonrly>(d => d.CaseNo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK14A_CASE_NO");
        });

        modelBuilder.Entity<T15PoDetail>(entity =>
        {
            entity.HasKey(e => new { e.CaseNo, e.ItemSrno }).HasName("PK_PO_DETAILS");

            entity.ToTable("T15_PO_DETAIL");

            entity.Property(e => e.CaseNo)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CASE_NO");
            entity.Property(e => e.ItemSrno)
                .HasPrecision(4)
                .HasColumnName("ITEM_SRNO");
            entity.Property(e => e.BasicValue)
                .HasColumnType("NUMBER(13,2)")
                .HasColumnName("BASIC_VALUE");
            entity.Property(e => e.ConsigneeCd)
                .HasPrecision(8)
                .HasColumnName("CONSIGNEE_CD");
            entity.Property(e => e.Datetime)
                .HasColumnType("DATE")
                .HasColumnName("DATETIME");
            entity.Property(e => e.DelvDt)
                .HasColumnType("DATE")
                .HasColumnName("DELV_DT");
            entity.Property(e => e.Discount)
                .HasColumnType("NUMBER(10,2)")
                .HasColumnName("DISCOUNT");
            entity.Property(e => e.DiscountPer)
                .HasColumnType("NUMBER(10,2)")
                .HasColumnName("DISCOUNT_PER");
            entity.Property(e => e.DiscountType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("DISCOUNT_TYPE");
            entity.Property(e => e.Excise)
                .HasColumnType("NUMBER(13,2)")
                .HasColumnName("EXCISE");
            entity.Property(e => e.ExcisePer)
                .HasColumnType("NUMBER(10,2)")
                .HasColumnName("EXCISE_PER");
            entity.Property(e => e.ExciseType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("EXCISE_TYPE");
            entity.Property(e => e.ExtDelvDt)
                .HasColumnType("DATE")
                .HasColumnName("EXT_DELV_DT");
            entity.Property(e => e.ItemCd)
                .HasMaxLength(7)
                .IsUnicode(false)
                .HasColumnName("ITEM_CD");
            entity.Property(e => e.ItemDesc)
                .HasMaxLength(400)
                .IsUnicode(false)
                .HasColumnName("ITEM_DESC");
            entity.Property(e => e.OtChargePer)
                .HasColumnType("NUMBER(10,2)")
                .HasColumnName("OT_CHARGE_PER");
            entity.Property(e => e.OtChargeType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("OT_CHARGE_TYPE");
            entity.Property(e => e.OtherCharges)
                .HasColumnType("NUMBER(10,2)")
                .HasColumnName("OTHER_CHARGES");
            entity.Property(e => e.PlNo)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("PL_NO");
            entity.Property(e => e.Qty)
                .HasColumnType("NUMBER(12,4)")
                .HasColumnName("QTY");
            entity.Property(e => e.Rate)
                .HasColumnType("NUMBER(11,2)")
                .HasColumnName("RATE");
            entity.Property(e => e.SalesTax)
                .HasColumnType("NUMBER(13,2)")
                .HasColumnName("SALES_TAX");
            entity.Property(e => e.SalesTaxPer)
                .HasColumnType("NUMBER(4,2)")
                .HasColumnName("SALES_TAX_PER");
            entity.Property(e => e.UomCd)
                .HasPrecision(3)
                .HasColumnName("UOM_CD");
            entity.Property(e => e.UserId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("USER_ID");
            entity.Property(e => e.Value)
                .HasColumnType("NUMBER(15,2)")
                .HasColumnName("VALUE");

            entity.HasOne(d => d.CaseNoNavigation).WithMany(p => p.T15PoDetails)
                .HasForeignKey(d => d.CaseNo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_15_CASE_NO");

            entity.HasOne(d => d.UomCdNavigation).WithMany(p => p.T15PoDetails)
                .HasForeignKey(d => d.UomCd)
                .HasConstraintName("FK15_UOM_CD");

            entity.HasOne(d => d.C).WithMany(p => p.T15PoDetails)
                .HasForeignKey(d => new { d.CaseNo, d.ConsigneeCd })
                .HasConstraintName("FK_PO_DETAILS");
        });

        modelBuilder.Entity<T16IcCancel>(entity =>
        {
            entity.HasKey(e => new { e.Region, e.BkNo, e.SetNo });

            entity.ToTable("T16_IC_CANCEL");

            entity.Property(e => e.Region)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("REGION");
            entity.Property(e => e.BkNo)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("BK_NO");
            entity.Property(e => e.SetNo)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("SET_NO");
            entity.Property(e => e.IcStatus)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("IC_STATUS");
            entity.Property(e => e.IssueToIecd)
                .HasPrecision(6)
                .HasColumnName("ISSUE_TO_IECD");
            entity.Property(e => e.Remarks)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("REMARKS");
            entity.Property(e => e.StatusDt)
                .HasColumnType("DATE")
                .HasColumnName("STATUS_DT");

            entity.HasOne(d => d.IssueToIecdNavigation).WithMany(p => p.T16IcCancels)
                .HasForeignKey(d => d.IssueToIecd)
                .HasConstraintName("FK_T16_IECD");
        });

        modelBuilder.Entity<T17CallRegister>(entity =>
        {
            entity.HasKey(e => new { e.CaseNo, e.CallRecvDt, e.CallSno }).HasName("PK_CALL_REGISTER");

            entity.ToTable("T17_CALL_REGISTER");

            entity.HasIndex(e => new { e.RegionCode, e.CallRecvDt, e.CallSno }, "UK_CALL_REGISTER").IsUnique();

            entity.Property(e => e.CaseNo)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CASE_NO");
            entity.Property(e => e.CallRecvDt)
                .HasColumnType("DATE")
                .HasColumnName("CALL_RECV_DT");
            entity.Property(e => e.CallSno)
                .HasPrecision(5)
                .HasColumnName("CALL_SNO");
            entity.Property(e => e.AutomaticCall)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("AUTOMATIC_CALL");
            entity.Property(e => e.BkNo)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("BK_NO");
            entity.Property(e => e.Bpo)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("BPO");
            entity.Property(e => e.CallCancelCharges)
                .HasPrecision(5)
                .HasColumnName("CALL_CANCEL_CHARGES");
            entity.Property(e => e.CallCancelStatus)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CALL_CANCEL_STATUS");
            entity.Property(e => e.CallInstallNo)
                .HasPrecision(4)
                .HasColumnName("CALL_INSTALL_NO");
            entity.Property(e => e.CallLetterDt)
                .HasColumnType("DATE")
                .HasColumnName("CALL_LETTER_DT");
            entity.Property(e => e.CallLetterNo)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("CALL_LETTER_NO");
            entity.Property(e => e.CallMarkDt)
                .HasColumnType("DATE")
                .HasColumnName("CALL_MARK_DT");
            entity.Property(e => e.CallRemarkStatus)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CALL_REMARK_STATUS");
            entity.Property(e => e.CallStatus)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CALL_STATUS");
            entity.Property(e => e.CallStatusDt)
                .HasColumnType("DATE")
                .HasColumnName("CALL_STATUS_DT");
            entity.Property(e => e.ClusterCode)
                .HasPrecision(3)
                .HasColumnName("CLUSTER_CODE");
            entity.Property(e => e.CoCd)
                .HasPrecision(3)
                .HasColumnName("CO_CD");
            entity.Property(e => e.CountDt)
                .HasPrecision(1)
                .HasColumnName("COUNT_DT");
            entity.Property(e => e.Datetime)
                .HasColumnType("DATE")
                .HasColumnName("DATETIME");
            entity.Property(e => e.DepartmentCode)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("DEPARTMENT_CODE");
            entity.Property(e => e.DtInspDesire)
                .HasColumnType("DATE")
                .HasColumnName("DT_INSP_DESIRE");
            entity.Property(e => e.ExpInspDt)
                .HasColumnType("DATE")
                .HasColumnName("EXP_INSP_DT");
            entity.Property(e => e.FifoVoilateReason)
                .HasMaxLength(400)
                .IsUnicode(false)
                .HasColumnName("FIFO_VOILATE_REASON");
            entity.Property(e => e.FinalOrStage)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("FINAL_OR_STAGE");
            entity.Property(e => e.Hologram)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("HOLOGRAM");
            entity.Property(e => e.IeCd)
                .HasPrecision(6)
                .HasColumnName("IE_CD");
            entity.Property(e => e.IrfcFunded)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("IRFC_FUNDED");
            entity.Property(e => e.ItemRdso)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ITEM_RDSO");
            entity.Property(e => e.LocalOrOuts)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("LOCAL_OR_OUTS");
            entity.Property(e => e.LotDp1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("LOT_DP_1");
            entity.Property(e => e.LotDp2)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("LOT_DP_2");
            entity.Property(e => e.MfgCd)
                .HasPrecision(6)
                .HasColumnName("MFG_CD");
            entity.Property(e => e.MfgPlace)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("MFG_PLACE");
            entity.Property(e => e.NewVendor)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("NEW_VENDOR");
            entity.Property(e => e.OnlineCall)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ONLINE_CALL");
            entity.Property(e => e.RecipientGstinNo)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("RECIPIENT_GSTIN_NO");
            entity.Property(e => e.RegionCode)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("REGION_CODE");
            entity.Property(e => e.RejCanCall)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("REJ_CAN_CALL");
            entity.Property(e => e.RejCharges)
                .HasColumnType("NUMBER(9,2)")
                .HasColumnName("REJ_CHARGES");
            entity.Property(e => e.Remarks)
                .HasMaxLength(400)
                .IsUnicode(false)
                .HasColumnName("REMARKS");
            entity.Property(e => e.SetNo)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("SET_NO");
            entity.Property(e => e.StaggeredDp)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("STAGGERED_DP");
            entity.Property(e => e.UpdateAllowed)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("UPDATE_ALLOWED");
            entity.Property(e => e.UserId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("USER_ID");
            entity.Property(e => e.VendApprovalFr)
                .HasColumnType("DATE")
                .HasColumnName("VEND_APPROVAL_FR");
            entity.Property(e => e.VendApprovalTo)
                .HasColumnType("DATE")
                .HasColumnName("VEND_APPROVAL_TO");
            entity.Property(e => e.VendRdso)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("VEND_RDSO");

            entity.HasOne(d => d.IeCdNavigation).WithMany(p => p.T17CallRegisters)
                .HasForeignKey(d => d.IeCd)
                .HasConstraintName("FK_CALL_IE_CD");

            entity.HasOne(d => d.MfgCdNavigation).WithMany(p => p.T17CallRegisters)
                .HasForeignKey(d => d.MfgCd)
                .HasConstraintName("FK_MFG_CD");

            entity.HasOne(d => d.RegionCodeNavigation).WithMany(p => p.T17CallRegisters)
                .HasForeignKey(d => d.RegionCode)
                .HasConstraintName("FK_CALL_REGION");
        });

        modelBuilder.Entity<T17CallRegisterSearchView>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("T17_CALL_REGISTER_SEARCH_VIEW");

            entity.Property(e => e.CallInstallNo)
                .HasPrecision(4)
                .HasColumnName("CALL_INSTALL_NO");
            entity.Property(e => e.CallLetterNo)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("CALL_LETTER_NO");
            entity.Property(e => e.CallRecvDt)
                .HasColumnType("DATE")
                .HasColumnName("CALL_RECV_DT");
            entity.Property(e => e.CallSno)
                .HasPrecision(5)
                .HasColumnName("CALL_SNO");
            entity.Property(e => e.CallStatus)
                .HasMaxLength(39)
                .IsUnicode(false)
                .HasColumnName("CALL_STATUS");
            entity.Property(e => e.CaseNo)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CASE_NO");
            entity.Property(e => e.IeSname)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("IE_SNAME");
            entity.Property(e => e.PoDt)
                .HasColumnType("DATE")
                .HasColumnName("PO_DT");
            entity.Property(e => e.PoNo)
                .HasMaxLength(75)
                .IsUnicode(false)
                .HasColumnName("PO_NO");
            entity.Property(e => e.Remarks)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("REMARKS");
            entity.Property(e => e.VendCd)
                .HasPrecision(6)
                .HasColumnName("VEND_CD");
            entity.Property(e => e.Vendor)
                .HasMaxLength(205)
                .IsUnicode(false)
                .HasColumnName("VENDOR");
        });

        modelBuilder.Entity<T18CallDetail>(entity =>
        {
            entity.HasKey(e => new { e.CaseNo, e.CallRecvDt, e.CallSno, e.ItemSrnoPo }).HasName("PK_CALL_DETAILS");

            entity.ToTable("T18_CALL_DETAILS");

            entity.Property(e => e.CaseNo)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CASE_NO");
            entity.Property(e => e.CallRecvDt)
                .HasColumnType("DATE")
                .HasColumnName("CALL_RECV_DT");
            entity.Property(e => e.CallSno)
                .HasPrecision(5)
                .HasColumnName("CALL_SNO");
            entity.Property(e => e.ItemSrnoPo)
                .HasPrecision(4)
                .HasColumnName("ITEM_SRNO_PO");
            entity.Property(e => e.ConsigneeCd)
                .HasPrecision(8)
                .HasColumnName("CONSIGNEE_CD");
            entity.Property(e => e.CumQtyPrevOffered)
                .HasColumnType("NUMBER(12,4)")
                .HasColumnName("CUM_QTY_PREV_OFFERED");
            entity.Property(e => e.CumQtyPrevPassed)
                .HasColumnType("NUMBER(12,4)")
                .HasColumnName("CUM_QTY_PREV_PASSED");
            entity.Property(e => e.Datetime)
                .HasColumnType("DATE")
                .HasColumnName("DATETIME");
            entity.Property(e => e.ItemDescPo)
                .HasMaxLength(400)
                .IsUnicode(false)
                .HasColumnName("ITEM_DESC_PO");
            entity.Property(e => e.QtyDue)
                .HasColumnType("NUMBER(12,4)")
                .HasColumnName("QTY_DUE");
            entity.Property(e => e.QtyOrdered)
                .HasColumnType("NUMBER(12,4)")
                .HasColumnName("QTY_ORDERED");
            entity.Property(e => e.QtyPassed)
                .HasColumnType("NUMBER(12,4)")
                .HasColumnName("QTY_PASSED");
            entity.Property(e => e.QtyRejected)
                .HasColumnType("NUMBER(12,4)")
                .HasColumnName("QTY_REJECTED");
            entity.Property(e => e.QtyToInsp)
                .HasColumnType("NUMBER(12,4)")
                .HasColumnName("QTY_TO_INSP");
            entity.Property(e => e.UserId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("USER_ID");

            entity.HasOne(d => d.ConsigneeCdNavigation).WithMany(p => p.T18CallDetails)
                .HasForeignKey(d => d.ConsigneeCd)
                .HasConstraintName("FK18_CONSIGNEE_CD");
        });

        modelBuilder.Entity<T19CallCancel>(entity =>
        {
            entity.HasKey(e => new { e.CaseNo, e.CallRecvDt, e.CallSno }).HasName("PK_CALL_CANCEL");

            entity.ToTable("T19_CALL_CANCEL");

            entity.Property(e => e.CaseNo)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CASE_NO");
            entity.Property(e => e.CallRecvDt)
                .HasColumnType("DATE")
                .HasColumnName("CALL_RECV_DT");
            entity.Property(e => e.CallSno)
                .HasPrecision(5)
                .HasColumnName("CALL_SNO");
            entity.Property(e => e.CancelCd1)
                .HasPrecision(2)
                .HasColumnName("CANCEL_CD_1");
            entity.Property(e => e.CancelCd10)
                .HasPrecision(2)
                .HasColumnName("CANCEL_CD_10");
            entity.Property(e => e.CancelCd11)
                .HasPrecision(2)
                .HasColumnName("CANCEL_CD_11");
            entity.Property(e => e.CancelCd2)
                .HasPrecision(2)
                .HasColumnName("CANCEL_CD_2");
            entity.Property(e => e.CancelCd3)
                .HasPrecision(2)
                .HasColumnName("CANCEL_CD_3");
            entity.Property(e => e.CancelCd4)
                .HasPrecision(2)
                .HasColumnName("CANCEL_CD_4");
            entity.Property(e => e.CancelCd5)
                .HasPrecision(2)
                .HasColumnName("CANCEL_CD_5");
            entity.Property(e => e.CancelCd6)
                .HasPrecision(2)
                .HasColumnName("CANCEL_CD_6");
            entity.Property(e => e.CancelCd7)
                .HasPrecision(2)
                .HasColumnName("CANCEL_CD_7");
            entity.Property(e => e.CancelCd8)
                .HasPrecision(2)
                .HasColumnName("CANCEL_CD_8");
            entity.Property(e => e.CancelCd9)
                .HasPrecision(2)
                .HasColumnName("CANCEL_CD_9");
            entity.Property(e => e.CancelDate)
                .HasColumnType("DATE")
                .HasColumnName("CANCEL_DATE");
            entity.Property(e => e.CancelDesc)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("CANCEL_DESC");
            entity.Property(e => e.Datetime)
                .HasColumnType("DATE")
                .HasColumnName("DATETIME");
            entity.Property(e => e.DocsSubmitted)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("DOCS_SUBMITTED");
            entity.Property(e => e.UserId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("USER_ID");

            entity.HasOne(d => d.Ca).WithOne(p => p.T19CallCancel)
                .HasForeignKey<T19CallCancel>(d => new { d.CaseNo, d.CallRecvDt, d.CallSno })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CALL_CANCEL");
        });

        modelBuilder.Entity<T20Ic>(entity =>
        {
            entity.HasKey(e => new { e.CaseNo, e.CallRecvDt, e.ConsigneeCd, e.CallSno });

            entity.ToTable("T20_IC");

            entity.Property(e => e.CaseNo)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CASE_NO");
            entity.Property(e => e.CallRecvDt)
                .HasColumnType("DATE")
                .HasColumnName("CALL_RECV_DT");
            entity.Property(e => e.ConsigneeCd)
                .HasPrecision(8)
                .HasColumnName("CONSIGNEE_CD");
            entity.Property(e => e.CallSno)
                .HasPrecision(5)
                .HasColumnName("CALL_SNO");
            entity.Property(e => e.AccGroup)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("ACC_GROUP");
            entity.Property(e => e.BillNo)
                .HasMaxLength(10)
                .IsUnicode(false)
                .ValueGeneratedOnAdd()
                .HasColumnName("BILL_NO");
            entity.Property(e => e.BkNo)
                .HasMaxLength(4)
                .IsUnicode(false)
                .ValueGeneratedOnAdd()
                .HasColumnName("BK_NO");
            entity.Property(e => e.BpoCd)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("BPO_CD");
            entity.Property(e => e.CallDt)
                .HasColumnType("DATE")
                .HasColumnName("CALL_DT");
            entity.Property(e => e.CallInstallNo)
                .HasPrecision(4)
                .HasColumnName("CALL_INSTALL_NO");
            entity.Property(e => e.CoCd)
                .HasPrecision(3)
                .HasColumnName("CO_CD");
            entity.Property(e => e.Datetime)
                .HasColumnType("DATE")
                .HasColumnName("DATETIME");
            entity.Property(e => e.FirstInspDt)
                .HasColumnType("DATE")
                .HasColumnName("FIRST_INSP_DT");
            entity.Property(e => e.FullPart)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("FULL_PART");
            entity.Property(e => e.IcDt)
                .HasColumnType("DATE")
                .HasColumnName("IC_DT");
            entity.Property(e => e.IcNo)
                .HasMaxLength(29)
                .IsUnicode(false)
                .HasColumnName("IC_NO");
            entity.Property(e => e.IcSubmitDt)
                .HasColumnType("DATE")
                .HasColumnName("IC_SUBMIT_DT");
            entity.Property(e => e.IcTypeId)
                .HasPrecision(1)
                .HasColumnName("IC_TYPE_ID");
            entity.Property(e => e.IeCd)
                .HasPrecision(6)
                .HasColumnName("IE_CD");
            entity.Property(e => e.IrfcBpoCd)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("IRFC_BPO_CD");
            entity.Property(e => e.IrfcFunded)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("IRFC_FUNDED");
            entity.Property(e => e.LastInspDt)
                .HasColumnType("DATE")
                .HasColumnName("LAST_INSP_DT");
            entity.Property(e => e.NoOfInsp)
                .HasPrecision(3)
                .HasColumnName("NO_OF_INSP");
            entity.Property(e => e.OtherInspDt)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("OTHER_INSP_DT");
            entity.Property(e => e.Photo)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("PHOTO");
            entity.Property(e => e.ReasonReject)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("REASON_REJECT");
            entity.Property(e => e.RecipientGstinNo)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("RECIPIENT_GSTIN_NO");
            entity.Property(e => e.SetNo)
                .HasMaxLength(3)
                .IsUnicode(false)
                .ValueGeneratedOnAdd()
                .HasColumnName("SET_NO");
            entity.Property(e => e.StampPattern)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("STAMP_PATTERN");
            entity.Property(e => e.StampPatternCd)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("STAMP_PATTERN_CD");
            entity.Property(e => e.UserId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("USER_ID");

            entity.HasOne(d => d.BillNoNavigation).WithMany(p => p.T20Ics)
                .HasForeignKey(d => d.BillNo)
                .HasConstraintName("FK20_BILL_NO");

            entity.HasOne(d => d.BpoCdNavigation).WithMany(p => p.T20Ics)
                .HasForeignKey(d => d.BpoCd)
                .HasConstraintName("FK20_BPO_CD");

            entity.HasOne(d => d.IcType).WithMany(p => p.T20Ics)
                .HasForeignKey(d => d.IcTypeId)
                .HasConstraintName("FK_IC_TYPE_ID");

            entity.HasOne(d => d.IeCdNavigation).WithMany(p => p.T20Ics)
                .HasForeignKey(d => d.IeCd)
                .HasConstraintName("FK_T20_IECD");

            entity.HasOne(d => d.Ca).WithMany(p => p.T20Ics)
                .HasForeignKey(d => new { d.CaseNo, d.CallRecvDt, d.CallSno })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_T20_IC");
        });

        modelBuilder.Entity<T21CallStatusCode>(entity =>
        {
            entity.HasKey(e => e.CallStatusCd).HasName("PK_CALL_STATUS_CD");

            entity.ToTable("T21_CALL_STATUS_CODES");

            entity.Property(e => e.CallStatusCd)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CALL_STATUS_CD");
            entity.Property(e => e.CallStatusColor)
                .HasMaxLength(7)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CALL_STATUS_COLOR");
            entity.Property(e => e.CallStatusDesc)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CALL_STATUS_DESC");
        });

        modelBuilder.Entity<T22Bill>(entity =>
        {
            entity.HasKey(e => e.BillNo).HasName("PK_BILL_NO");

            entity.ToTable("T22_BILL");

            entity.Property(e => e.BillNo)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("BILL_NO");
            entity.Property(e => e.AckDt)
                .HasColumnType("DATE")
                .HasColumnName("ACK_DT");
            entity.Property(e => e.AckNo)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ACK_NO");
            entity.Property(e => e.AdvBill)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ADV_BILL");
            entity.Property(e => e.AmountReceived)
                .HasColumnType("NUMBER(13,2)")
                .HasColumnName("AMOUNT_RECEIVED");
            entity.Property(e => e.BillAmount)
                .HasColumnType("NUMBER(13,2)")
                .HasColumnName("BILL_AMOUNT");
            entity.Property(e => e.BillAmtCleared)
                .HasColumnType("NUMBER(13,2)")
                .HasColumnName("BILL_AMT_CLEARED");
            entity.Property(e => e.BillDt)
                .HasColumnType("DATE")
                .HasColumnName("BILL_DT");
            entity.Property(e => e.BillFinalised)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("BILL_FINALISED");
            entity.Property(e => e.BillResentCount)
                .HasPrecision(1)
                .HasColumnName("BILL_RESENT_COUNT");
            entity.Property(e => e.BillResentStatus)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("BILL_RESENT_STATUS");
            entity.Property(e => e.BillStatus)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("BILL_STATUS");
            entity.Property(e => e.CaseNo)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CASE_NO");
            entity.Property(e => e.Cgst)
                .HasColumnType("NUMBER(10,2)")
                .HasColumnName("CGST");
            entity.Property(e => e.CnoteAmount)
                .HasColumnType("NUMBER(13,2)")
                .HasColumnName("CNOTE_AMOUNT");
            entity.Property(e => e.CnoteBillNo)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("CNOTE_BILL_NO");
            entity.Property(e => e.CreditDocId)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("CREDIT_DOC_ID");
            entity.Property(e => e.Datetime)
                .HasColumnType("DATE")
                .HasColumnName("DATETIME");
            entity.Property(e => e.DigBillGenDt)
                .HasColumnType("DATE")
                .HasColumnName("DIG_BILL_GEN_DT");
            entity.Property(e => e.EduCess)
                .HasColumnType("NUMBER(10,2)")
                .HasColumnName("EDU_CESS");
            entity.Property(e => e.FeeRate)
                .HasColumnType("NUMBER(11,4)")
                .HasColumnName("FEE_RATE");
            entity.Property(e => e.FeeType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("FEE_TYPE");
            entity.Property(e => e.Igst)
                .HasColumnType("NUMBER(10,2)")
                .HasColumnName("IGST");
            entity.Property(e => e.InspFee)
                .HasColumnType("NUMBER(13,2)")
                .HasColumnName("INSP_FEE");
            entity.Property(e => e.InvoiceNo)
                .HasMaxLength(14)
                .IsUnicode(false)
                .HasColumnName("INVOICE_NO");
            entity.Property(e => e.InvoiceSuppDocs)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("INVOICE_SUPP_DOCS");
            entity.Property(e => e.IrnNo)
                .HasMaxLength(64)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("IRN_NO");
            entity.Property(e => e.KrishiKalyanCess)
                .HasColumnType("NUMBER(10,2)")
                .HasColumnName("KRISHI_KALYAN_CESS");
            entity.Property(e => e.LoRemarks)
                .HasMaxLength(400)
                .IsUnicode(false)
                .HasColumnName("LO_REMARKS");
            entity.Property(e => e.MaterialValue)
                .HasColumnType("NUMBER(15,2)")
                .HasColumnName("MATERIAL_VALUE");
            entity.Property(e => e.MaxFee)
                .HasPrecision(11)
                .HasColumnName("MAX_FEE");
            entity.Property(e => e.MinFee)
                .HasPrecision(11)
                .HasColumnName("MIN_FEE");
            entity.Property(e => e.QrCode)
                .HasColumnType("NCLOB")
                .HasColumnName("QR_CODE");
            entity.Property(e => e.Remarks)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("REMARKS");
            entity.Property(e => e.RetentionMoney)
                .HasColumnType("NUMBER(13,2)")
                .HasColumnName("RETENTION_MONEY");
            entity.Property(e => e.ScannedStatus)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("SCANNED_STATUS");
            entity.Property(e => e.SentToSap)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("SENT_TO_SAP");
            entity.Property(e => e.ServTaxRate)
                .HasColumnType("NUMBER(5,2)")
                .HasColumnName("SERV_TAX_RATE");
            entity.Property(e => e.ServiceTax)
                .HasColumnType("NUMBER(11,2)")
                .HasColumnName("SERVICE_TAX");
            entity.Property(e => e.Sgst)
                .HasColumnType("NUMBER(10,2)")
                .HasColumnName("SGST");
            entity.Property(e => e.SheCess)
                .HasColumnType("NUMBER(10,2)")
                .HasColumnName("SHE_CESS");
            entity.Property(e => e.SwachhBharatCess)
                .HasColumnType("NUMBER(10,2)")
                .HasColumnName("SWACHH_BHARAT_CESS");
            entity.Property(e => e.TaxType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("TAX_TYPE");
            entity.Property(e => e.Tds)
                .HasColumnType("NUMBER(13,2)")
                .HasColumnName("TDS");
            entity.Property(e => e.TdsCgst)
                .HasColumnType("NUMBER(10,2)")
                .HasColumnName("TDS_CGST");
            entity.Property(e => e.TdsDt)
                .HasColumnType("DATE")
                .HasColumnName("TDS_DT");
            entity.Property(e => e.TdsIgst)
                .HasColumnType("NUMBER(10,2)")
                .HasColumnName("TDS_IGST");
            entity.Property(e => e.TdsSgst)
                .HasColumnType("NUMBER(10,2)")
                .HasColumnName("TDS_SGST");
            entity.Property(e => e.UserId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("USER_ID");
            entity.Property(e => e.WriteOffAmt)
                .HasColumnType("NUMBER(13,2)")
                .HasColumnName("WRITE_OFF_AMT");
        });

        modelBuilder.Entity<T23BillItem>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("T23_BILL_ITEMS");

            entity.Property(e => e.BasicValue)
                .HasColumnType("NUMBER(13,2)")
                .HasColumnName("BASIC_VALUE");
            entity.Property(e => e.BillNo)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("BILL_NO");
            entity.Property(e => e.Discount)
                .HasColumnType("NUMBER(10,2)")
                .HasColumnName("DISCOUNT");
            entity.Property(e => e.DiscountPer)
                .HasColumnType("NUMBER(10,2)")
                .HasColumnName("DISCOUNT_PER");
            entity.Property(e => e.DiscountType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("DISCOUNT_TYPE");
            entity.Property(e => e.Excise)
                .HasColumnType("NUMBER(11,2)")
                .HasColumnName("EXCISE");
            entity.Property(e => e.ExcisePer)
                .HasColumnType("NUMBER(10,2)")
                .HasColumnName("EXCISE_PER");
            entity.Property(e => e.ExciseType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("EXCISE_TYPE");
            entity.Property(e => e.ItemDesc)
                .HasMaxLength(400)
                .IsUnicode(false)
                .HasColumnName("ITEM_DESC");
            entity.Property(e => e.ItemSrno)
                .HasPrecision(4)
                .HasColumnName("ITEM_SRNO");
            entity.Property(e => e.OtChargePer)
                .HasColumnType("NUMBER(10,2)")
                .HasColumnName("OT_CHARGE_PER");
            entity.Property(e => e.OtChargeType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("OT_CHARGE_TYPE");
            entity.Property(e => e.OtherCharges)
                .HasColumnType("NUMBER(10,2)")
                .HasColumnName("OTHER_CHARGES");
            entity.Property(e => e.Qty)
                .HasColumnType("NUMBER(12,4)")
                .HasColumnName("QTY");
            entity.Property(e => e.Rate)
                .HasColumnType("NUMBER(11,2)")
                .HasColumnName("RATE");
            entity.Property(e => e.SalesTax)
                .HasColumnType("NUMBER(13,2)")
                .HasColumnName("SALES_TAX");
            entity.Property(e => e.SalesTaxPer)
                .HasColumnType("NUMBER(10,2)")
                .HasColumnName("SALES_TAX_PER");
            entity.Property(e => e.UomCd)
                .HasPrecision(3)
                .HasColumnName("UOM_CD");
            entity.Property(e => e.Value)
                .HasColumnType("NUMBER(15,2)")
                .HasColumnName("VALUE");

            entity.HasOne(d => d.BillNoNavigation).WithMany()
                .HasForeignKey(d => d.BillNo)
                .HasConstraintName("FK_BILL_NO");

            entity.HasOne(d => d.UomCdNavigation).WithMany()
                .HasForeignKey(d => d.UomCd)
                .HasConstraintName("FK23_UOM_CD");
        });

        modelBuilder.Entity<T24Rv>(entity =>
        {
            entity.HasKey(e => e.VchrNo).HasName("PK_VCHR_NO");

            entity.ToTable("T24_RV");

            entity.Property(e => e.VchrNo)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("VCHR_NO");
            entity.Property(e => e.BankCd)
                .HasPrecision(3)
                .HasColumnName("BANK_CD");
            entity.Property(e => e.VchrDt)
                .HasColumnType("DATE")
                .HasColumnName("VCHR_DT");
            entity.Property(e => e.VchrType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("VCHR_TYPE");
        });

        modelBuilder.Entity<T252709>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("T25_2709");

            entity.Property(e => e.AccCd)
                .HasPrecision(4)
                .HasColumnName("ACC_CD");
            entity.Property(e => e.Amount)
                .HasColumnType("NUMBER(12,2)")
                .HasColumnName("AMOUNT");
            entity.Property(e => e.AmountPosted)
                .HasColumnType("NUMBER(12,2)")
                .HasColumnName("AMOUNT_POSTED");
            entity.Property(e => e.AmtTransferred)
                .HasColumnType("NUMBER(12,2)")
                .HasColumnName("AMT_TRANSFERRED");
            entity.Property(e => e.BankCd)
                .HasPrecision(3)
                .HasColumnName("BANK_CD");
            entity.Property(e => e.BpoCd)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("BPO_CD");
            entity.Property(e => e.ChqDt)
                .HasColumnType("DATE")
                .HasColumnName("CHQ_DT");
            entity.Property(e => e.ChqNo)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("CHQ_NO");
            entity.Property(e => e.RegionCode)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("REGION_CODE");
            entity.Property(e => e.Sno)
                .HasPrecision(4)
                .HasColumnName("SNO");
            entity.Property(e => e.Suspense)
                .HasColumnType("NUMBER(12,2)")
                .HasColumnName("SUSPENSE");
            entity.Property(e => e.VchrDt)
                .HasColumnType("DATE")
                .HasColumnName("VCHR_DT");
            entity.Property(e => e.VchrNo)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("VCHR_NO");
        });

        modelBuilder.Entity<T25RvDetail>(entity =>
        {
            entity.HasKey(e => new { e.BankCd, e.ChqNo, e.ChqDt }).HasName("PK25_RV_DETAILS");

            entity.ToTable("T25_RV_DETAILS");

            entity.HasIndex(e => new { e.VchrNo, e.Sno }, "UQ25_RV_DETAILS").IsUnique();

            entity.Property(e => e.BankCd)
                .HasPrecision(3)
                .HasColumnName("BANK_CD");
            entity.Property(e => e.ChqNo)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("CHQ_NO");
            entity.Property(e => e.ChqDt)
                .HasColumnType("DATE")
                .HasColumnName("CHQ_DT");
            entity.Property(e => e.AccCd)
                .HasPrecision(4)
                .HasColumnName("ACC_CD");
            entity.Property(e => e.Amount)
                .HasColumnType("NUMBER(12,2)")
                .HasColumnName("AMOUNT");
            entity.Property(e => e.AmountAdjusted)
                .HasColumnType("NUMBER(12,2)")
                .HasColumnName("AMOUNT_ADJUSTED");
            entity.Property(e => e.AmtTransferred)
                .HasColumnType("NUMBER(12,2)")
                .HasColumnName("AMT_TRANSFERRED");
            entity.Property(e => e.BpoCd)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("BPO_CD");
            entity.Property(e => e.BpoType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("BPO_TYPE");
            entity.Property(e => e.CaseNo)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CASE_NO");
            entity.Property(e => e.Datetime)
                .HasColumnType("DATE")
                .HasColumnName("DATETIME");
            entity.Property(e => e.Narration)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NARRATION");
            entity.Property(e => e.PostDt)
                .HasColumnType("DATE")
                .HasColumnName("POST_DT");
            entity.Property(e => e.SampleNo)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("SAMPLE_NO");
            entity.Property(e => e.Sno)
                .HasPrecision(4)
                .HasColumnName("SNO");
            entity.Property(e => e.Status)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("STATUS");
            entity.Property(e => e.SuspenseAmt)
                .HasColumnType("NUMBER(12,2)")
                .HasColumnName("SUSPENSE_AMT");
            entity.Property(e => e.UserId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("USER_ID");
            entity.Property(e => e.VchrNo)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("VCHR_NO");

            entity.HasOne(d => d.CaseNoNavigation).WithMany(p => p.T25RvDetails)
                .HasForeignKey(d => d.CaseNo)
                .HasConstraintName("FK25_CASE_NO");

            entity.HasOne(d => d.VchrNoNavigation).WithMany(p => p.T25RvDetails)
                .HasForeignKey(d => d.VchrNo)
                .HasConstraintName("FK_VCHR_NO");
        });

        modelBuilder.Entity<T26ChequePosting>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("T26_CHEQUE_POSTING");

            entity.HasIndex(e => new { e.BankCd, e.ChqNo, e.ChqDt, e.BillNo }, "UQ26_CHEQUE_POSTING").IsUnique();

            entity.Property(e => e.AmountCleared)
                .HasColumnType("NUMBER(13,2)")
                .HasColumnName("AMOUNT_CLEARED");
            entity.Property(e => e.BankCd)
                .HasPrecision(3)
                .HasColumnName("BANK_CD");
            entity.Property(e => e.BillAmount)
                .HasColumnType("NUMBER(13,2)")
                .HasColumnName("BILL_AMOUNT");
            entity.Property(e => e.BillNo)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("BILL_NO");
            entity.Property(e => e.BpoCd)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("BPO_CD");
            entity.Property(e => e.ChqDt)
                .HasColumnType("DATE")
                .HasColumnName("CHQ_DT");
            entity.Property(e => e.ChqNo)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("CHQ_NO");
            entity.Property(e => e.Datetime)
                .HasColumnType("DATE")
                .HasColumnName("DATETIME");
            entity.Property(e => e.PostingDt)
                .HasColumnType("DATE")
                .HasColumnName("POSTING_DT");
            entity.Property(e => e.UserId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("USER_ID");

            entity.HasOne(d => d.T25RvDetail).WithMany()
                .HasForeignKey(d => new { d.BankCd, d.ChqNo, d.ChqDt })
                .HasConstraintName("FK26_CHEQUE_POSTING");
        });

        modelBuilder.Entity<T27Jv>(entity =>
        {
            entity.HasKey(e => e.VchrNo).HasName("PK27_VCHR_NO");

            entity.ToTable("T27_JV");

            entity.HasIndex(e => new { e.BankCd, e.ChqNo, e.ChqDt }, "UQ27_JV").IsUnique();

            entity.Property(e => e.VchrNo)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("VCHR_NO");
            entity.Property(e => e.BankCd)
                .HasPrecision(3)
                .HasColumnName("BANK_CD");
            entity.Property(e => e.ChqDt)
                .HasColumnType("DATE")
                .HasColumnName("CHQ_DT");
            entity.Property(e => e.ChqNo)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("CHQ_NO");
            entity.Property(e => e.RvSno)
                .HasPrecision(4)
                .HasColumnName("RV_SNO");
            entity.Property(e => e.RvVchrNo)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("RV_VCHR_NO");
            entity.Property(e => e.VchrDt)
                .HasColumnType("DATE")
                .HasColumnName("VCHR_DT");
        });

        modelBuilder.Entity<T28SapRealisation>(entity =>
        {
            entity.HasKey(e => new { e.RealisationPer, e.RegionCode, e.ClientType }).HasName("PK28_SAP_REALISATION");

            entity.ToTable("T28_SAP_REALISATION");

            entity.Property(e => e.RealisationPer)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("REALISATION_PER");
            entity.Property(e => e.RegionCode)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("REGION_CODE");
            entity.Property(e => e.ClientType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CLIENT_TYPE");
            entity.Property(e => e.Amount)
                .HasColumnType("NUMBER(13,2)")
                .HasColumnName("AMOUNT");
            entity.Property(e => e.Datetime)
                .HasColumnType("DATE")
                .HasColumnName("DATETIME");
            entity.Property(e => e.UserId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("USER_ID");
        });

        modelBuilder.Entity<T29JvDetail>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("T29_JV_DETAILS");

            entity.HasIndex(e => new { e.VchrNo, e.AccCd }, "UQ29_JV_DETAILS").IsUnique();

            entity.Property(e => e.AccCd)
                .HasPrecision(4)
                .HasColumnName("ACC_CD");
            entity.Property(e => e.Amount)
                .HasColumnType("NUMBER(12,2)")
                .HasColumnName("AMOUNT");
            entity.Property(e => e.IuAdvDt)
                .HasColumnType("DATE")
                .HasColumnName("IU_ADV_DT");
            entity.Property(e => e.IuAdvNo)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("IU_ADV_NO");
            entity.Property(e => e.Narration)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NARRATION");
            entity.Property(e => e.VchrNo)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("VCHR_NO");

            entity.HasOne(d => d.VchrNoNavigation).WithMany()
                .HasForeignKey(d => d.VchrNo)
                .HasConstraintName("SYS_C008167");
        });

        modelBuilder.Entity<T30IcReceived>(entity =>
        {
            entity.HasKey(e => new { e.Region, e.BkNo, e.SetNo }).HasName("PK_T30_IC_RECEIPT");

            entity.ToTable("T30_IC_RECEIVED");

            entity.Property(e => e.Region)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("REGION");
            entity.Property(e => e.BkNo)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("BK_NO");
            entity.Property(e => e.SetNo)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("SET_NO");
            entity.Property(e => e.BillNo)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("BILL_NO");
            entity.Property(e => e.Datetime)
                .HasColumnType("DATE")
                .HasColumnName("DATETIME");
            entity.Property(e => e.IcSubmitDt)
                .HasColumnType("DATE")
                .HasColumnName("IC_SUBMIT_DT");
            entity.Property(e => e.IeCd)
                .HasPrecision(4)
                .HasColumnName("IE_CD");
            entity.Property(e => e.Remarks)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("REMARKS");
            entity.Property(e => e.RemarksDt)
                .HasColumnType("DATE")
                .HasColumnName("REMARKS_DT");
            entity.Property(e => e.UserId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("USER_ID");

            entity.HasOne(d => d.BillNoNavigation).WithMany(p => p.T30IcReceiveds)
                .HasForeignKey(d => d.BillNo)
                .HasConstraintName("FK30_BILL_NO");
        });

        modelBuilder.Entity<T31HologramIssued>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("T31_HOLOGRAM_ISSUED");

            entity.Property(e => e.Datetime)
                .HasColumnType("DATE")
                .HasColumnName("DATETIME");
            entity.Property(e => e.HgIecd)
                .HasPrecision(6)
                .HasColumnName("HG_IECD");
            entity.Property(e => e.HgIssueDt)
                .HasColumnType("DATE")
                .HasColumnName("HG_ISSUE_DT");
            entity.Property(e => e.HgNoFr)
                .HasMaxLength(7)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("HG_NO_FR");
            entity.Property(e => e.HgNoTo)
                .HasMaxLength(7)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("HG_NO_TO");
            entity.Property(e => e.HgRegion)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("HG_REGION");
            entity.Property(e => e.UserId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("USER_ID");

            entity.HasOne(d => d.HgIecdNavigation).WithMany()
                .HasForeignKey(d => d.HgIecd)
                .HasConstraintName("FK31_IECD");
        });

        modelBuilder.Entity<T32ClientLogin>(entity =>
        {
            entity.HasKey(e => e.Mobile).HasName("PK_CLIENT_ID");

            entity.ToTable("T32_CLIENT_LOGIN");

            entity.Property(e => e.Mobile)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MOBILE");
            entity.Property(e => e.Designation)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("DESIGNATION");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.Organisation)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("ORGANISATION");
            entity.Property(e => e.OrgnType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ORGN_TYPE");
            entity.Property(e => e.Pwd)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("PWD");
            entity.Property(e => e.Status)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("STATUS");
            entity.Property(e => e.Unit)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("UNIT");
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("USER_NAME");
        });

        modelBuilder.Entity<T33HologramAccountal>(entity =>
        {
            entity.HasKey(e => new { e.CaseNo, e.CallRecvDt, e.ConsigneeCd, e.CallSno, e.RecNo }).HasName("PK33_HOLOGRAM_ACCOUNTAL");

            entity.ToTable("T33_HOLOGRAM_ACCOUNTAL");

            entity.Property(e => e.CaseNo)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CASE_NO");
            entity.Property(e => e.CallRecvDt)
                .HasColumnType("DATE")
                .HasColumnName("CALL_RECV_DT");
            entity.Property(e => e.ConsigneeCd)
                .HasPrecision(8)
                .HasColumnName("CONSIGNEE_CD");
            entity.Property(e => e.CallSno)
                .HasPrecision(5)
                .HasColumnName("CALL_SNO");
            entity.Property(e => e.RecNo)
                .HasPrecision(2)
                .HasColumnName("REC_NO");
            entity.Property(e => e.Datetime)
                .HasColumnType("DATE")
                .HasColumnName("DATETIME");
            entity.Property(e => e.HgNoIcDoc)
                .HasMaxLength(7)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("HG_NO_IC_DOC");
            entity.Property(e => e.HgNoIcFr)
                .HasMaxLength(7)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("HG_NO_IC_FR");
            entity.Property(e => e.HgNoIcTo)
                .HasMaxLength(7)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("HG_NO_IC_TO");
            entity.Property(e => e.HgNoMaterialFr)
                .HasMaxLength(7)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("HG_NO_MATERIAL_FR");
            entity.Property(e => e.HgNoMaterialTo)
                .HasMaxLength(7)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("HG_NO_MATERIAL_TO");
            entity.Property(e => e.HgNoOtFr)
                .HasMaxLength(7)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("HG_NO_OT_FR");
            entity.Property(e => e.HgNoOtTo)
                .HasMaxLength(7)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("HG_NO_OT_TO");
            entity.Property(e => e.HgNoSampleFr)
                .HasMaxLength(7)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("HG_NO_SAMPLE_FR");
            entity.Property(e => e.HgNoSampleTo)
                .HasMaxLength(7)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("HG_NO_SAMPLE_TO");
            entity.Property(e => e.HgNoTestFr)
                .HasMaxLength(7)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("HG_NO_TEST_FR");
            entity.Property(e => e.HgNoTestTo)
                .HasMaxLength(7)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("HG_NO_TEST_TO");
            entity.Property(e => e.HgOtDesc)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("HG_OT_DESC");
            entity.Property(e => e.HgRegion)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("HG_REGION");
            entity.Property(e => e.UserId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("USER_ID");
        });

        modelBuilder.Entity<T34RailPrice>(entity =>
        {
            entity.HasKey(e => e.RailId).HasName("PK34_RAIL_ID");

            entity.ToTable("T34_RAIL_PRICE");

            entity.Property(e => e.RailId)
                .HasPrecision(2)
                .HasColumnName("RAIL_ID");
            entity.Property(e => e.Datetime)
                .HasColumnType("DATE")
                .HasColumnName("DATETIME");
            entity.Property(e => e.RailDesc)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("RAIL_DESC");
            entity.Property(e => e.RailLengthMeter)
                .HasPrecision(3)
                .HasColumnName("RAIL_LENGTH_METER");
            entity.Property(e => e.UserId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("USER_ID");
        });

        modelBuilder.Entity<T35RailPriceDetail>(entity =>
        {
            entity.HasKey(e => new { e.RailId, e.IdSrno }).HasName("FK35_RAIL_PRICE_DETAIL");

            entity.ToTable("T35_RAIL_PRICE_DETAILS");

            entity.Property(e => e.RailId)
                .HasPrecision(2)
                .HasColumnName("RAIL_ID");
            entity.Property(e => e.IdSrno)
                .HasPrecision(2)
                .HasColumnName("ID_SRNO");
            entity.Property(e => e.Datetime)
                .HasColumnType("DATE")
                .HasColumnName("DATETIME");
            entity.Property(e => e.PackingCharge)
                .HasPrecision(10)
                .HasColumnName("PACKING_CHARGE");
            entity.Property(e => e.PriceDateFr)
                .HasColumnType("DATE")
                .HasColumnName("PRICE_DATE_FR");
            entity.Property(e => e.PriceDateTo)
                .HasColumnType("DATE")
                .HasColumnName("PRICE_DATE_TO");
            entity.Property(e => e.RailPricePerMt)
                .HasPrecision(10)
                .HasColumnName("RAIL_PRICE_PER_MT");
            entity.Property(e => e.UserId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("USER_ID");

            entity.HasOne(d => d.Rail).WithMany(p => p.T35RailPriceDetails)
                .HasForeignKey(d => d.RailId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK35_RAIL_ID");
        });

        modelBuilder.Entity<T36Bill>(entity =>
        {
            entity.HasKey(e => e.BillNo).HasName("PK36_BILL_NO");

            entity.ToTable("T36_BILL");

            entity.Property(e => e.BillNo)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("BILL_NO");
            entity.Property(e => e.BasePrice)
                .HasColumnType("NUMBER(12,2)")
                .HasColumnName("BASE_PRICE");
            entity.Property(e => e.BillAmount)
                .HasColumnType("NUMBER(13,2)")
                .HasColumnName("BILL_AMOUNT");
            entity.Property(e => e.BillDt)
                .HasColumnType("DATE")
                .HasColumnName("BILL_DT");
            entity.Property(e => e.Bpo)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("BPO");
            entity.Property(e => e.Consignee)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("CONSIGNEE");
            entity.Property(e => e.Datetime)
                .HasColumnType("DATE")
                .HasColumnName("DATETIME");
            entity.Property(e => e.EduCess)
                .HasColumnType("NUMBER(10,2)")
                .HasColumnName("EDU_CESS");
            entity.Property(e => e.Excise)
                .HasColumnType("NUMBER(12,2)")
                .HasColumnName("EXCISE");
            entity.Property(e => e.ExcisePer)
                .HasColumnType("NUMBER(5,2)")
                .HasColumnName("EXCISE_PER");
            entity.Property(e => e.FeeRate)
                .HasColumnType("NUMBER(4,2)")
                .HasColumnName("FEE_RATE");
            entity.Property(e => e.IcDt)
                .HasColumnType("DATE")
                .HasColumnName("IC_DT");
            entity.Property(e => e.IcNo)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("IC_NO");
            entity.Property(e => e.InspFee)
                .HasColumnType("NUMBER(13,2)")
                .HasColumnName("INSP_FEE");
            entity.Property(e => e.Laying)
                .HasColumnType("NUMBER(12,2)")
                .HasColumnName("LAYING");
            entity.Property(e => e.MaterialValue)
                .HasColumnType("NUMBER(15,2)")
                .HasColumnName("MATERIAL_VALUE");
            entity.Property(e => e.PoDt)
                .HasColumnType("DATE")
                .HasColumnName("PO_DT");
            entity.Property(e => e.PoNo)
                .HasMaxLength(70)
                .IsUnicode(false)
                .HasColumnName("PO_NO");
            entity.Property(e => e.Purchaser)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("PURCHASER");
            entity.Property(e => e.Qty)
                .HasColumnType("NUMBER(12,4)")
                .HasColumnName("QTY");
            entity.Property(e => e.RailDesc)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("RAIL_DESC");
            entity.Property(e => e.Region)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("REGION");
            entity.Property(e => e.Remarks)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("REMARKS");
            entity.Property(e => e.RlyNonrly)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("RLY_NONRLY");
            entity.Property(e => e.SalesTax)
                .HasColumnType("NUMBER(12,2)")
                .HasColumnName("SALES_TAX");
            entity.Property(e => e.SalesTaxPer)
                .HasColumnType("NUMBER(5,2)")
                .HasColumnName("SALES_TAX_PER");
            entity.Property(e => e.ServTaxRate)
                .HasColumnType("NUMBER(5,2)")
                .HasColumnName("SERV_TAX_RATE");
            entity.Property(e => e.ServiceTax)
                .HasColumnType("NUMBER(11,2)")
                .HasColumnName("SERVICE_TAX");
            entity.Property(e => e.SheCess)
                .HasColumnType("NUMBER(10,2)")
                .HasColumnName("SHE_CESS");
            entity.Property(e => e.TotBaseValue)
                .HasColumnType("NUMBER(15,2)")
                .HasColumnName("TOT_BASE_VALUE");
            entity.Property(e => e.UserId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("USER_ID");
        });

        modelBuilder.Entity<T37ClientLogginLog>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("T37_CLIENT_LOGGIN_LOG");

            entity.Property(e => e.LogginTime)
                .HasColumnType("DATE")
                .HasColumnName("LOGGIN_TIME");
            entity.Property(e => e.Mobile)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MOBILE");
            entity.Property(e => e.Otp)
                .HasPrecision(4)
                .HasColumnName("OTP");
            entity.Property(e => e.OtpExpTime)
                .HasColumnType("DATE")
                .HasColumnName("OTP_EXP_TIME");
            entity.Property(e => e.OtpGenTime)
                .HasColumnType("DATE")
                .HasColumnName("OTP_GEN_TIME");
            entity.Property(e => e.Status)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("STATUS");
        });

        modelBuilder.Entity<T38DefectCode>(entity =>
        {
            entity.HasKey(e => e.DefectCd).HasName("PK38_DEFECT_CD");

            entity.ToTable("T38_DEFECT_CODES");

            entity.Property(e => e.DefectCd)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("DEFECT_CD");
            entity.Property(e => e.DefectDesc)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("DEFECT_DESC");
        });

        modelBuilder.Entity<T39JiStatusCode>(entity =>
        {
            entity.HasKey(e => e.JiStatusCd).HasName("PK39_JI_STATUS");

            entity.ToTable("T39_JI_STATUS_CODES");

            entity.Property(e => e.JiStatusCd)
                .HasPrecision(2)
                .HasColumnName("JI_STATUS_CD");
            entity.Property(e => e.JiStatusDesc)
                .HasMaxLength(35)
                .IsUnicode(false)
                .HasColumnName("JI_STATUS_DESC");
        });

        modelBuilder.Entity<T40ConsigneeComplaint>(entity =>
        {
            entity.HasKey(e => e.ComplaintId).HasName("PK40_COMPLAINT_ID");

            entity.ToTable("T40_CONSIGNEE_COMPLAINTS");

            entity.HasIndex(e => e.JiSno, "UK40_JI_NO").IsUnique();

            entity.Property(e => e.ComplaintId)
                .HasMaxLength(12)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("COMPLAINT_ID");
            entity.Property(e => e.Action)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("ACTION");
            entity.Property(e => e.ActionProposed)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ACTION_PROPOSED");
            entity.Property(e => e.ActionProposedDt)
                .HasColumnType("DATE")
                .HasColumnName("ACTION_PROPOSED_DT");
            entity.Property(e => e.AnyOther)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("ANY_OTHER");
            entity.Property(e => e.BkNo)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("BK_NO");
            entity.Property(e => e.CapaStatus)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("CAPA_STATUS");
            entity.Property(e => e.CaseNo)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CASE_NO");
            entity.Property(e => e.ChksheetStatus)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CHKSHEET_STATUS");
            entity.Property(e => e.CompRecvRegion)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("COMP_RECV_REGION");
            entity.Property(e => e.ComplaintDt)
                .HasColumnType("DATE")
                .HasColumnName("COMPLAINT_DT");
            entity.Property(e => e.ConclusionDt)
                .HasColumnType("DATE")
                .HasColumnName("CONCLUSION_DT");
            entity.Property(e => e.ConsigneeCd)
                .HasPrecision(8)
                .HasColumnName("CONSIGNEE_CD");
            entity.Property(e => e.DandarStatus)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("DANDAR_STATUS");
            entity.Property(e => e.Datetime)
                .HasColumnType("DATE")
                .HasColumnName("DATETIME");
            entity.Property(e => e.DefectCd)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("DEFECT_CD");
            entity.Property(e => e.IeCd)
                .HasPrecision(4)
                .HasColumnName("IE_CD");
            entity.Property(e => e.IeCoCd)
                .HasPrecision(3)
                .HasColumnName("IE_CO_CD");
            entity.Property(e => e.IeJiRemarks)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("IE_JI_REMARKS");
            entity.Property(e => e.IeJiRemarksDt)
                .HasColumnType("DATE")
                .HasColumnName("IE_JI_REMARKS_DT");
            entity.Property(e => e.InspRegion)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("INSP_REGION");
            entity.Property(e => e.ItemDesc)
                .HasMaxLength(400)
                .IsUnicode(false)
                .HasColumnName("ITEM_DESC");
            entity.Property(e => e.ItemSrnoPo)
                .HasPrecision(4)
                .HasColumnName("ITEM_SRNO_PO");
            entity.Property(e => e.JiApprovalDt)
                .HasColumnType("DATE")
                .HasColumnName("JI_APPROVAL_DT");
            entity.Property(e => e.JiApprovedBy)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("JI_APPROVED_BY");
            entity.Property(e => e.JiDt)
                .HasColumnType("DATE")
                .HasColumnName("JI_DT");
            entity.Property(e => e.JiFixDt)
                .HasColumnType("DATE")
                .HasColumnName("JI_FIX_DT");
            entity.Property(e => e.JiIeCd)
                .HasPrecision(4)
                .HasColumnName("JI_IE_CD");
            entity.Property(e => e.JiIeRemarks)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("JI_IE_REMARKS");
            entity.Property(e => e.JiIeRemarksDt)
                .HasColumnType("DATE")
                .HasColumnName("JI_IE_REMARKS_DT");
            entity.Property(e => e.JiRegion)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("JI_REGION");
            entity.Property(e => e.JiRequired)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("JI_REQUIRED");
            entity.Property(e => e.JiSno)
                .HasMaxLength(13)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("JI_SNO");
            entity.Property(e => e.JiStatusCd)
                .HasPrecision(2)
                .HasColumnName("JI_STATUS_CD");
            entity.Property(e => e.NoJiReason)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("NO_JI_REASON");
            entity.Property(e => e.PenaltyDt)
                .HasColumnType("DATE")
                .HasColumnName("PENALTY_DT");
            entity.Property(e => e.PenaltyType)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("PENALTY_TYPE");
            entity.Property(e => e.QtyOffered)
                .HasColumnType("NUMBER(12,4)")
                .HasColumnName("QTY_OFFERED");
            entity.Property(e => e.QtyRejected)
                .HasColumnType("NUMBER(12,4)")
                .HasColumnName("QTY_REJECTED");
            entity.Property(e => e.Rate)
                .HasColumnType("NUMBER(13,4)")
                .HasColumnName("RATE");
            entity.Property(e => e.RejMemoDt)
                .HasColumnType("DATE")
                .HasColumnName("REJ_MEMO_DT");
            entity.Property(e => e.RejMemoNo)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("REJ_MEMO_NO");
            entity.Property(e => e.RejectionReason)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("REJECTION_REASON");
            entity.Property(e => e.RejectionValue)
                .HasColumnType("NUMBER(15,2)")
                .HasColumnName("REJECTION_VALUE");
            entity.Property(e => e.Remarks)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("REMARKS");
            entity.Property(e => e.RootCauseAnalysis)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("ROOT_CAUSE_ANALYSIS");
            entity.Property(e => e.SetNo)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("SET_NO");
            entity.Property(e => e.Status)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("STATUS");
            entity.Property(e => e.TechRef)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("TECH_REF");
            entity.Property(e => e.UomCd)
                .HasPrecision(3)
                .HasColumnName("UOM_CD");
            entity.Property(e => e.UserId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("USER_ID");
            entity.Property(e => e.VendCd)
                .HasPrecision(6)
                .HasColumnName("VEND_CD");

            entity.HasOne(d => d.CaseNoNavigation).WithMany(p => p.T40ConsigneeComplaints)
                .HasForeignKey(d => d.CaseNo)
                .HasConstraintName("FK40_PO_NO");

            entity.HasOne(d => d.ConsigneeCdNavigation).WithMany(p => p.T40ConsigneeComplaints)
                .HasForeignKey(d => d.ConsigneeCd)
                .HasConstraintName("FK40_CONSIGNEE_CD");

            entity.HasOne(d => d.DefectCdNavigation).WithMany(p => p.T40ConsigneeComplaints)
                .HasForeignKey(d => d.DefectCd)
                .HasConstraintName("FK40_DEFECT_CD");

            entity.HasOne(d => d.JiStatusCdNavigation).WithMany(p => p.T40ConsigneeComplaints)
                .HasForeignKey(d => d.JiStatusCd)
                .HasConstraintName("FK40_JI_STATUS");

            entity.HasOne(d => d.VendCdNavigation).WithMany(p => p.T40ConsigneeComplaints)
                .HasForeignKey(d => d.VendCd)
                .HasConstraintName("FK40_VEND_CD");
        });

        modelBuilder.Entity<T41NcMaster>(entity =>
        {
            entity.HasKey(e => new { e.CaseNo, e.CallRecvDt, e.CallSno, e.ItemSrnoPo }).HasName("PK_T41_NC");

            entity.ToTable("T41_NC_MASTER");

            entity.HasIndex(e => e.NcNo, "T41_NC_MASTER").IsUnique();

            entity.Property(e => e.CaseNo)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CASE_NO");
            entity.Property(e => e.CallRecvDt)
                .HasColumnType("DATE")
                .HasColumnName("CALL_RECV_DT");
            entity.Property(e => e.CallSno)
                .HasPrecision(5)
                .HasColumnName("CALL_SNO");
            entity.Property(e => e.ItemSrnoPo)
                .HasPrecision(4)
                .HasColumnName("ITEM_SRNO_PO");
            entity.Property(e => e.BkNo)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("BK_NO");
            entity.Property(e => e.CoCd)
                .HasPrecision(3)
                .HasColumnName("CO_CD");
            entity.Property(e => e.ConsigneeCd)
                .HasPrecision(8)
                .HasColumnName("CONSIGNEE_CD");
            entity.Property(e => e.Datetime)
                .HasColumnType("DATE")
                .HasColumnName("DATETIME");
            entity.Property(e => e.IcDt)
                .HasColumnType("DATE")
                .HasColumnName("IC_DT");
            entity.Property(e => e.IcNo)
                .HasMaxLength(29)
                .IsUnicode(false)
                .HasColumnName("IC_NO");
            entity.Property(e => e.IeCd)
                .HasPrecision(4)
                .HasColumnName("IE_CD");
            entity.Property(e => e.ItemDescPo)
                .HasMaxLength(400)
                .IsUnicode(false)
                .HasColumnName("ITEM_DESC_PO");
            entity.Property(e => e.NcDt)
                .HasColumnType("DATE")
                .HasColumnName("NC_DT");
            entity.Property(e => e.NcNo)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("NC_NO");
            entity.Property(e => e.PoDt)
                .HasColumnType("DATE")
                .HasColumnName("PO_DT");
            entity.Property(e => e.PoNo)
                .HasMaxLength(75)
                .IsUnicode(false)
                .HasColumnName("PO_NO");
            entity.Property(e => e.QtyPassed)
                .HasColumnType("NUMBER(12,4)")
                .HasColumnName("QTY_PASSED");
            entity.Property(e => e.RegionCode)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("REGION_CODE");
            entity.Property(e => e.SetNo)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("SET_NO");
            entity.Property(e => e.UserId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("USER_ID");
            entity.Property(e => e.VendCd)
                .HasPrecision(6)
                .HasColumnName("VEND_CD");

            entity.HasOne(d => d.ConsigneeCdNavigation).WithMany(p => p.T41NcMasters)
                .HasForeignKey(d => d.ConsigneeCd)
                .HasConstraintName("FK41_CONSIGNEE_CD");

            entity.HasOne(d => d.VendCdNavigation).WithMany(p => p.T41NcMasters)
                .HasForeignKey(d => d.VendCd)
                .HasConstraintName("FK41_VEND_CD");
        });

        modelBuilder.Entity<T42NcDetail>(entity =>
        {
            entity.HasKey(e => new { e.NcNo, e.NcCdSno });

            entity.ToTable("T42_NC_DETAIL");

            entity.Property(e => e.NcNo)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("NC_NO");
            entity.Property(e => e.NcCdSno)
                .HasPrecision(1)
                .HasColumnName("NC_CD_SNO");
            entity.Property(e => e.CoFinalRemarks1)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("CO_FINAL_REMARKS1");
            entity.Property(e => e.CoFinalRemarks1Dt)
                .HasColumnType("DATE")
                .HasColumnName("CO_FINAL_REMARKS1_DT");
            entity.Property(e => e.Datetime)
                .HasColumnType("DATE")
                .HasColumnName("DATETIME");
            entity.Property(e => e.IeAction1)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("IE_ACTION1");
            entity.Property(e => e.IeAction1Dt)
                .HasColumnType("DATE")
                .HasColumnName("IE_ACTION1_DT");
            entity.Property(e => e.NcCd)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("NC_CD");
            entity.Property(e => e.NcDescOthers)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("NC_DESC_OTHERS");
            entity.Property(e => e.UserId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("USER_ID");

            entity.HasOne(d => d.NcCdNavigation).WithMany(p => p.T42NcDetails)
                .HasForeignKey(d => d.NcCd)
                .HasConstraintName("FK_NC_CD");
        });

        modelBuilder.Entity<T43CallReturn>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("T43_CALL_RETURN");

            entity.HasIndex(e => e.ReturnNo, "T43_RETURN_NO").IsUnique();

            entity.Property(e => e.CallLetterDt)
                .HasColumnType("DATE")
                .HasColumnName("CALL_LETTER_DT");
            entity.Property(e => e.CallLetterNo)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("CALL_LETTER_NO");
            entity.Property(e => e.Datetime)
                .HasColumnType("DATE")
                .HasColumnName("DATETIME");
            entity.Property(e => e.DtOfReciept)
                .HasColumnType("DATE")
                .HasColumnName("DT_OF_RECIEPT");
            entity.Property(e => e.ReturnDt)
                .HasColumnType("DATE")
                .HasColumnName("RETURN_DT");
            entity.Property(e => e.ReturnNo)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("RETURN_NO");
            entity.Property(e => e.ReturnReason1)
                .HasPrecision(1)
                .HasColumnName("RETURN_REASON_1");
            entity.Property(e => e.ReturnReason2)
                .HasPrecision(1)
                .HasColumnName("RETURN_REASON_2");
            entity.Property(e => e.ReturnReason3)
                .HasPrecision(1)
                .HasColumnName("RETURN_REASON_3");
            entity.Property(e => e.ReturnReason4)
                .HasPrecision(1)
                .HasColumnName("RETURN_REASON_4");
            entity.Property(e => e.ReturnReason5)
                .HasPrecision(1)
                .HasColumnName("RETURN_REASON_5");
            entity.Property(e => e.ReturnReason6)
                .HasPrecision(1)
                .HasColumnName("RETURN_REASON_6");
            entity.Property(e => e.ReturnReason7)
                .HasPrecision(1)
                .HasColumnName("RETURN_REASON_7");
            entity.Property(e => e.ReturnReason8)
                .HasPrecision(1)
                .HasColumnName("RETURN_REASON_8");
            entity.Property(e => e.ReturnReason9)
                .HasPrecision(1)
                .HasColumnName("RETURN_REASON_9");
            entity.Property(e => e.ReturnRemarks)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("RETURN_REMARKS");
            entity.Property(e => e.RlyCd)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("RLY_CD");
            entity.Property(e => e.RlyNonrly)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("RLY_NONRLY");
            entity.Property(e => e.UserId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("USER_ID");
            entity.Property(e => e.VendCd)
                .HasPrecision(6)
                .HasColumnName("VEND_CD");
            entity.Property(e => e.VendContactNo)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("VEND_CONTACT_NO");
            entity.Property(e => e.VendEmail)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("VEND_EMAIL");
        });

        modelBuilder.Entity<T44SuperSurprise>(entity =>
        {
            entity.HasKey(e => new { e.CaseNo, e.CallRecvDt, e.CallSno }).HasName("PK_SUPER_SURPRISE");

            entity.ToTable("T44_SUPER_SURPRISE");

            entity.HasIndex(e => e.SuperSurpriseNo, "UK_SUPER_SURPRISE_NO").IsUnique();

            entity.Property(e => e.CaseNo)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CASE_NO");
            entity.Property(e => e.CallRecvDt)
                .HasColumnType("DATE")
                .HasColumnName("CALL_RECV_DT");
            entity.Property(e => e.CallSno)
                .HasPrecision(5)
                .HasColumnName("CALL_SNO");
            entity.Property(e => e.CoCd)
                .HasPrecision(3)
                .HasColumnName("CO_CD");
            entity.Property(e => e.Datetime)
                .HasColumnType("DATE")
                .HasColumnName("DATETIME");
            entity.Property(e => e.Discrepancy)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("DISCREPANCY");
            entity.Property(e => e.IeCd)
                .HasPrecision(4)
                .HasColumnName("IE_CD");
            entity.Property(e => e.ItemDesc)
                .HasMaxLength(400)
                .IsUnicode(false)
                .HasColumnName("ITEM_DESC");
            entity.Property(e => e.NameScopeItem)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("NAME_SCOPE_ITEM");
            entity.Property(e => e.Outcome)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("OUTCOME");
            entity.Property(e => e.PreIntRej)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("PRE_INT_REJ");
            entity.Property(e => e.SbuHeadRemarks)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("SBU_HEAD_REMARKS");
            entity.Property(e => e.SuperSurpriseDt)
                .HasColumnType("DATE")
                .HasColumnName("SUPER_SURPRISE_DT");
            entity.Property(e => e.SuperSurpriseNo)
                .HasMaxLength(7)
                .IsUnicode(false)
                .HasColumnName("SUPER_SURPRISE_NO");
            entity.Property(e => e.UserId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("USER_ID");

            entity.HasOne(d => d.Ca).WithOne(p => p.T44SuperSurprise)
                .HasForeignKey<T44SuperSurprise>(d => new { d.CaseNo, d.CallRecvDt, d.CallSno })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SUPER_SURPRISE");
        });

        modelBuilder.Entity<T45ClaimMaster>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("T45_CLAIM_MASTER_PK");

            entity.ToTable("T45_CLAIM_MASTER");

            entity.HasIndex(e => e.ClaimNo, "T45_CLAIM_NO").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("\"IBSDEV\".\"T45_CLAIM_MASTER_SEQ\".\"NEXTVAL\"")
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ID");
            entity.Property(e => e.ClaimDt)
                .HasColumnType("DATE")
                .HasColumnName("CLAIM_DT");
            entity.Property(e => e.ClaimNo)
                .HasMaxLength(11)
                .IsUnicode(false)
                .HasColumnName("CLAIM_NO");
            entity.Property(e => e.Datetime)
                .HasColumnType("DATE")
                .HasColumnName("DATETIME");
            entity.Property(e => e.IeCd)
                .HasPrecision(6)
                .HasColumnName("IE_CD");
            entity.Property(e => e.PaymentVchrDt)
                .HasColumnType("DATE")
                .HasColumnName("PAYMENT_VCHR_DT");
            entity.Property(e => e.PaymentVchrNo)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("PAYMENT_VCHR_NO");
            entity.Property(e => e.PeriodFrom)
                .HasPrecision(6)
                .HasColumnName("PERIOD_FROM");
            entity.Property(e => e.PeriodTo)
                .HasPrecision(6)
                .HasColumnName("PERIOD_TO");
            entity.Property(e => e.ReceiveDt)
                .HasColumnType("DATE")
                .HasColumnName("RECEIVE_DT");
            entity.Property(e => e.RegionCode)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("REGION_CODE");
            entity.Property(e => e.UserId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("USER_ID");

            entity.HasOne(d => d.IeCdNavigation).WithMany(p => p.T45ClaimMasters)
                .HasForeignKey(d => d.IeCd)
                .HasConstraintName("FK_T45_IE");
        });

        modelBuilder.Entity<T46ClaimDetail>(entity =>
        {
            entity.HasKey(e => new { e.ClaimNo, e.ClaimHead });

            entity.ToTable("T46_CLAIM_DETAIL");

            entity.Property(e => e.ClaimNo)
                .HasMaxLength(11)
                .IsUnicode(false)
                .HasColumnName("CLAIM_NO");
            entity.Property(e => e.ClaimHead)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("CLAIM_HEAD");
            entity.Property(e => e.AmtAdmitted)
                .HasPrecision(6)
                .HasColumnName("AMT_ADMITTED");
            entity.Property(e => e.AmtClaimed)
                .HasPrecision(6)
                .HasColumnName("AMT_CLAIMED");
            entity.Property(e => e.AmtDisallowed)
                .HasPrecision(6)
                .HasColumnName("AMT_DISALLOWED");
            entity.Property(e => e.Datetime)
                .HasColumnType("DATE")
                .HasColumnName("DATETIME");
            entity.Property(e => e.Id)
                .HasDefaultValueSql("\"IBSDEV\".\"T46_CLAIM_DETAIL_SEQ\".\"NEXTVAL\"")
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ID");
            entity.Property(e => e.Remarks)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("REMARKS");
            entity.Property(e => e.UserId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("USER_ID");

            entity.HasOne(d => d.ClaimNoNavigation).WithMany(p => p.T46ClaimDetails)
                .HasPrincipalKey(p => p.ClaimNo)
                .HasForeignKey(d => d.ClaimNo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CLAIM_NO");
        });

        modelBuilder.Entity<T47IeWorkPlan>(entity =>
        {
            entity.HasKey(e => new { e.CaseNo, e.CallRecvDt, e.CallSno, e.VisitDt });

            entity.ToTable("T47_IE_WORK_PLAN");

            entity.Property(e => e.CaseNo)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CASE_NO");
            entity.Property(e => e.CallRecvDt)
                .HasColumnType("DATE")
                .HasColumnName("CALL_RECV_DT");
            entity.Property(e => e.CallSno)
                .HasPrecision(5)
                .HasColumnName("CALL_SNO");
            entity.Property(e => e.VisitDt)
                .HasColumnType("DATE")
                .HasColumnName("VISIT_DT");
            entity.Property(e => e.CoCd)
                .HasPrecision(3)
                .HasColumnName("CO_CD");
            entity.Property(e => e.Datetime)
                .HasColumnType("DATE")
                .HasColumnName("DATETIME");
            entity.Property(e => e.Id)
                .HasDefaultValueSql("\"IBSDEV\".\"T47_IE_WORK_PLAN_SEQ\".\"NEXTVAL\"")
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ID");
            entity.Property(e => e.IeCd)
                .HasPrecision(6)
                .HasColumnName("IE_CD");
            entity.Property(e => e.MfgCd)
                .HasPrecision(6)
                .HasColumnName("MFG_CD");
            entity.Property(e => e.MfgPlace)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("MFG_PLACE");
            entity.Property(e => e.RegionCode)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("REGION_CODE");
            entity.Property(e => e.UserId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("USER_ID");

            entity.HasOne(d => d.IeCdNavigation).WithMany(p => p.T47IeWorkPlans)
                .HasForeignKey(d => d.IeCd)
                .HasConstraintName("FK_T47_IE_CD");

            entity.HasOne(d => d.MfgCdNavigation).WithMany(p => p.T47IeWorkPlans)
                .HasForeignKey(d => d.MfgCd)
                .HasConstraintName("FK_T47_MFG_CD");

            entity.HasOne(d => d.Ca).WithMany(p => p.T47IeWorkPlans)
                .HasForeignKey(d => new { d.CaseNo, d.CallRecvDt, d.CallSno })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_T47_IE_WORK_PLAN");
        });

        modelBuilder.Entity<T48NiIeWorkPlan>(entity =>
        {
            entity.HasKey(e => new { e.IeCd, e.NiWorkDt });

            entity.ToTable("T48_NI_IE_WORK_PLAN");

            entity.Property(e => e.IeCd)
                .HasPrecision(6)
                .HasColumnName("IE_CD");
            entity.Property(e => e.NiWorkDt)
                .HasColumnType("DATE")
                .HasColumnName("NI_WORK_DT");
            entity.Property(e => e.CoCd)
                .HasPrecision(3)
                .HasColumnName("CO_CD");
            entity.Property(e => e.Datetime)
                .HasColumnType("DATE")
                .HasColumnName("DATETIME");
            entity.Property(e => e.NiOtherDesc)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("NI_OTHER_DESC");
            entity.Property(e => e.NiWorkCd)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("NI_WORK_CD");
            entity.Property(e => e.RegionCode)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("REGION_CODE");
            entity.Property(e => e.UserId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("USER_ID");

            entity.HasOne(d => d.IeCdNavigation).WithMany(p => p.T48NiIeWorkPlans)
                .HasForeignKey(d => d.IeCd)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_T48_IE_CD");
        });

        modelBuilder.Entity<T49IcPhotoEnclosed>(entity =>
        {
            entity.HasKey(e => new { e.CaseNo, e.BkNo, e.SetNo });

            entity.ToTable("T49_IC_PHOTO_ENCLOSED");

            entity.Property(e => e.CaseNo)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CASE_NO");
            entity.Property(e => e.BkNo)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("BK_NO");
            entity.Property(e => e.SetNo)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("SET_NO");
            entity.Property(e => e.CallRecvDt)
                .HasColumnType("DATE")
                .HasColumnName("CALL_RECV_DT");
            entity.Property(e => e.CallSno)
                .HasPrecision(5)
                .HasColumnName("CALL_SNO");
            entity.Property(e => e.ConsigneeCd)
                .HasPrecision(8)
                .HasColumnName("CONSIGNEE_CD");
            entity.Property(e => e.Datetime)
                .HasColumnType("DATE")
                .HasColumnName("DATETIME");
            entity.Property(e => e.File1)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("FILE_1");
            entity.Property(e => e.File10)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("FILE_10");
            entity.Property(e => e.File2)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("FILE_2");
            entity.Property(e => e.File3)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("FILE_3");
            entity.Property(e => e.File4)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("FILE_4");
            entity.Property(e => e.File5)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("FILE_5");
            entity.Property(e => e.File6)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("FILE_6");
            entity.Property(e => e.File7)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("FILE_7");
            entity.Property(e => e.File8)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("FILE_8");
            entity.Property(e => e.File9)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("FILE_9");
            entity.Property(e => e.IcPhoto)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("IC_PHOTO");
            entity.Property(e => e.IcPhotoA1)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("IC_PHOTO_A1");
            entity.Property(e => e.IcPhotoA2)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("IC_PHOTO_A2");
            entity.Property(e => e.PhotoSource)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("PHOTO_SOURCE");
            entity.Property(e => e.UserId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("USER_ID");
        });

        modelBuilder.Entity<T50LabRegister>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("T50_LAB_REGISTER_PK");

            entity.ToTable("T50_LAB_REGISTER");

            entity.HasIndex(e => e.SampleRegNo, "T50_LAB_REGISTER").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("\"IBSDEV\".\"T50_LAB_REGISTER_SEQ\".\"NEXTVAL\"")
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ID");
            entity.Property(e => e.AmountRecieved)
                .HasPrecision(13)
                .HasColumnName("AMOUNT_RECIEVED");
            entity.Property(e => e.CallRecvDt)
                .HasColumnType("DATE")
                .HasColumnName("CALL_RECV_DT");
            entity.Property(e => e.CallSno)
                .HasPrecision(5)
                .HasColumnName("CALL_SNO");
            entity.Property(e => e.CaseNo)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CASE_NO");
            entity.Property(e => e.CodeDt)
                .HasColumnType("DATE")
                .HasColumnName("CODE_DT");
            entity.Property(e => e.CodeNo)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("CODE_NO");
            entity.Property(e => e.Datetime)
                .HasColumnType("DATE")
                .HasColumnName("DATETIME");
            entity.Property(e => e.IeCd)
                .HasPrecision(4)
                .HasColumnName("IE_CD");
            entity.Property(e => e.SampleDispatchDt)
                .HasColumnType("DATE")
                .HasColumnName("SAMPLE_DISPATCH_DT");
            entity.Property(e => e.SampleDrawlDt)
                .HasColumnType("DATE")
                .HasColumnName("SAMPLE_DRAWL_DT");
            entity.Property(e => e.SampleRecieptDt)
                .HasColumnType("DATE")
                .HasColumnName("SAMPLE_RECIEPT_DT");
            entity.Property(e => e.SampleRegDt)
                .HasColumnType("DATE")
                .HasColumnName("SAMPLE_REG_DT");
            entity.Property(e => e.SampleRegNo)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("SAMPLE_REG_NO");
            entity.Property(e => e.Tds)
                .HasColumnType("NUMBER(11,2)")
                .HasColumnName("TDS");
            entity.Property(e => e.TdsDt)
                .HasColumnType("DATE")
                .HasColumnName("TDS_DT");
            entity.Property(e => e.TestingType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("TESTING_TYPE");
            entity.Property(e => e.TotalHandlingCharges)
                .HasPrecision(10)
                .HasColumnName("TOTAL_HANDLING_CHARGES");
            entity.Property(e => e.TotalLabCharges)
                .HasPrecision(13)
                .HasColumnName("TOTAL_LAB_CHARGES");
            entity.Property(e => e.TotalServiceTax)
                .HasPrecision(10)
                .HasColumnName("TOTAL_SERVICE_TAX");
            entity.Property(e => e.TotalTestingFee)
                .HasPrecision(11)
                .HasColumnName("TOTAL_TESTING_FEE");
            entity.Property(e => e.UserId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("USER_ID");
            entity.Property(e => e.VendCd)
                .HasPrecision(6)
                .HasColumnName("VEND_CD");

            entity.HasOne(d => d.Ca).WithMany(p => p.T50LabRegisters)
                .HasForeignKey(d => new { d.CaseNo, d.CallRecvDt, d.CallSno })
                .HasConstraintName("FK_T50_LAB_REGISTER");
        });

        modelBuilder.Entity<T51LabRegisterDetail>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("T51_LAB_REGISTER_DETAIL");

            entity.HasIndex(e => new { e.SampleRegNo, e.Sno }, "T51_LAB_REGISTER_DETAIL").IsUnique();

            entity.Property(e => e.AmountReceived)
                .HasColumnType("NUMBER(13,2)")
                .HasColumnName("AMOUNT_RECEIVED");
            entity.Property(e => e.Datetime)
                .HasColumnType("DATE")
                .HasColumnName("DATETIME");
            entity.Property(e => e.HandlingCharges)
                .HasColumnType("NUMBER(10,2)")
                .HasColumnName("HANDLING_CHARGES");
            entity.Property(e => e.ItemDesc)
                .HasMaxLength(400)
                .IsUnicode(false)
                .HasColumnName("ITEM_DESC");
            entity.Property(e => e.LabId)
                .HasPrecision(6)
                .HasColumnName("LAB_ID");
            entity.Property(e => e.PaymentId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("PAYMENT_ID");
            entity.Property(e => e.Qty)
                .HasPrecision(3)
                .HasColumnName("QTY");
            entity.Property(e => e.Remarks)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("REMARKS");
            entity.Property(e => e.SampleDispatchedToLabDt)
                .HasColumnType("DATE")
                .HasColumnName("SAMPLE_DISPATCHED_TO_LAB_DT");
            entity.Property(e => e.SampleRegNo)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("SAMPLE_REG_NO");
            entity.Property(e => e.ServiceTax)
                .HasColumnType("NUMBER(11,2)")
                .HasColumnName("SERVICE_TAX");
            entity.Property(e => e.Sno)
                .HasPrecision(2)
                .HasColumnName("SNO");
            entity.Property(e => e.Test)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("TEST");
            entity.Property(e => e.TestCategoryCd)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("TEST_CATEGORY_CD");
            entity.Property(e => e.TestReportRecDt)
                .HasColumnType("DATE")
                .HasColumnName("TEST_REPORT_REC_DT");
            entity.Property(e => e.TestReportReqDt)
                .HasColumnType("DATE")
                .HasColumnName("TEST_REPORT_REQ_DT");
            entity.Property(e => e.TestStatus)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("TEST_STATUS");
            entity.Property(e => e.TestingFee)
                .HasColumnType("NUMBER(11,2)")
                .HasColumnName("TESTING_FEE");
            entity.Property(e => e.UserId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("USER_ID");

            entity.HasOne(d => d.Lab).WithMany()
                .HasForeignKey(d => d.LabId)
                .HasConstraintName("FK_T65_LABORATORY_MASTER");

            entity.HasOne(d => d.SampleRegNoNavigation).WithMany()
                .HasPrincipalKey(p => p.SampleRegNo)
                .HasForeignKey(d => d.SampleRegNo)
                .HasConstraintName("FK_SAMPLE_REG_NO");

            entity.HasOne(d => d.TestCategoryCdNavigation).WithMany()
                .HasForeignKey(d => d.TestCategoryCd)
                .HasConstraintName("FK_TEST_CATEGORY_CD");
        });

        modelBuilder.Entity<T52LabPosting>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("T52_LAB_POSTING");

            entity.HasIndex(e => new { e.BankCd, e.ChqNo, e.ChqDt, e.SampleRegNo }, "UQ52_CHEQUE_POSTING").IsUnique();

            entity.Property(e => e.AmtCleared)
                .HasColumnType("NUMBER(13,2)")
                .HasColumnName("AMT_CLEARED");
            entity.Property(e => e.BankCd)
                .HasPrecision(3)
                .HasColumnName("BANK_CD");
            entity.Property(e => e.ChqDt)
                .HasColumnType("DATE")
                .HasColumnName("CHQ_DT");
            entity.Property(e => e.ChqNo)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("CHQ_NO");
            entity.Property(e => e.Datetime)
                .HasColumnType("DATE")
                .HasColumnName("DATETIME");
            entity.Property(e => e.SampleRegNo)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("SAMPLE_REG_NO");
            entity.Property(e => e.TotalLabCharges)
                .HasPrecision(13)
                .HasColumnName("TOTAL_LAB_CHARGES");
            entity.Property(e => e.UserId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("USER_ID");

            entity.HasOne(d => d.T25RvDetail).WithMany()
                .HasForeignKey(d => new { d.BankCd, d.ChqNo, d.ChqDt })
                .HasConstraintName("FK52_CHEQUE_POSTING");
        });

        modelBuilder.Entity<T53VigilanceCasesMaster>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("T53_VIGILANCE_CASES_MASTER_PK");

            entity.ToTable("T53_VIGILANCE_CASES_MASTER");

            entity.HasIndex(e => e.RefRegNo, "T53_VIGILANCE_CASES_MASTER").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("\"IBSDEV\".\"T53_VIGILANCE_CASES_MASTER_SEQ\".\"NEXTVAL\"")
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ID");
            entity.Property(e => e.ActionProposed)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ACTION_PROPOSED");
            entity.Property(e => e.ActionProposedDt)
                .HasColumnType("DATE")
                .HasColumnName("ACTION_PROPOSED_DT");
            entity.Property(e => e.Datetime)
                .HasColumnType("DATE")
                .HasColumnName("DATETIME");
            entity.Property(e => e.FinalAction)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("FINAL_ACTION");
            entity.Property(e => e.FinalActionDt)
                .HasColumnType("DATE")
                .HasColumnName("FINAL_ACTION_DT");
            entity.Property(e => e.PrelimInvDetails)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("PRELIM_INV_DETAILS");
            entity.Property(e => e.RefDetails)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("REF_DETAILS");
            entity.Property(e => e.RefDt)
                .HasColumnType("DATE")
                .HasColumnName("REF_DT");
            entity.Property(e => e.RefNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("REF_NO");
            entity.Property(e => e.RefRegNo)
                .HasMaxLength(7)
                .IsUnicode(false)
                .HasColumnName("REF_REG_NO");
            entity.Property(e => e.RefReplyDt)
                .HasColumnType("DATE")
                .HasColumnName("REF_REPLY_DT");
            entity.Property(e => e.UserId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("USER_ID");
        });

        modelBuilder.Entity<T54VigilanceCasesDetail>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("T54_VIGILANCE_CASES_DETAILS");

            entity.HasIndex(e => new { e.RefRegNo, e.CaseNo, e.BkNo, e.SetNo }, "T54_VIGI_CASES_DETAILS_CASE").IsUnique();

            entity.HasIndex(e => new { e.RefRegNo, e.Sno }, "T54_VIGI_CASES_DETAILS_SNO").IsUnique();

            entity.Property(e => e.BillDt)
                .HasColumnType("DATE")
                .HasColumnName("BILL_DT");
            entity.Property(e => e.BillNo)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("BILL_NO");
            entity.Property(e => e.BkNo)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("BK_NO");
            entity.Property(e => e.BpoCd)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("BPO_CD");
            entity.Property(e => e.CaseNo)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CASE_NO");
            entity.Property(e => e.ConsigneeCd)
                .HasPrecision(8)
                .HasColumnName("CONSIGNEE_CD");
            entity.Property(e => e.Datetime)
                .HasColumnType("DATE")
                .HasColumnName("DATETIME");
            entity.Property(e => e.IeCd)
                .HasPrecision(4)
                .HasColumnName("IE_CD");
            entity.Property(e => e.RefRegNo)
                .HasMaxLength(7)
                .IsUnicode(false)
                .HasColumnName("REF_REG_NO");
            entity.Property(e => e.SetNo)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("SET_NO");
            entity.Property(e => e.Sno)
                .HasPrecision(2)
                .HasColumnName("SNO");
            entity.Property(e => e.UserId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("USER_ID");

            entity.HasOne(d => d.RefRegNoNavigation).WithMany()
                .HasPrincipalKey(p => p.RefRegNo)
                .HasForeignKey(d => d.RefRegNo)
                .HasConstraintName("FK_T54_VIGILANCE_CASES_DETAILS");
        });

        modelBuilder.Entity<T55LabInvoice>(entity =>
        {
            entity.HasKey(e => e.InvoiceNo).HasName("PK55_INVOICE_NO");

            entity.ToTable("T55_LAB_INVOICE");

            entity.Property(e => e.InvoiceNo)
                .HasMaxLength(14)
                .IsUnicode(false)
                .HasColumnName("INVOICE_NO");
            entity.Property(e => e.AckDt)
                .HasColumnType("DATE")
                .HasColumnName("ACK_DT");
            entity.Property(e => e.AckNo)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ACK_NO");
            entity.Property(e => e.BillAmount)
                .HasColumnType("NUMBER(13,2)")
                .HasColumnName("BILL_AMOUNT");
            entity.Property(e => e.BillFinalised)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("BILL_FINALISED");
            entity.Property(e => e.BpoCd)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("BPO_CD");
            entity.Property(e => e.CNote)
                .HasMaxLength(14)
                .IsUnicode(false)
                .HasColumnName("C_NOTE");
            entity.Property(e => e.CaseNo)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CASE_NO");
            entity.Property(e => e.CreditId)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("CREDIT_ID");
            entity.Property(e => e.CustomerRefNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CUSTOMER_REF_NO");
            entity.Property(e => e.Datetime)
                .HasColumnType("DATE")
                .HasColumnName("DATETIME");
            entity.Property(e => e.IeCd)
                .HasPrecision(4)
                .HasColumnName("IE_CD");
            entity.Property(e => e.IncType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("INC_TYPE");
            entity.Property(e => e.InvSCity)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("INV_S_CITY");
            entity.Property(e => e.InvoiceDt)
                .HasColumnType("DATE")
                .HasColumnName("INVOICE_DT");
            entity.Property(e => e.IrnNo)
                .HasMaxLength(64)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("IRN_NO");
            entity.Property(e => e.QrCode)
                .HasColumnType("NCLOB")
                .HasColumnName("QR_CODE");
            entity.Property(e => e.RecipientGstinNo)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("RECIPIENT_GSTIN_NO");
            entity.Property(e => e.RegionCode)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("REGION_CODE");
            entity.Property(e => e.SampleRegNo)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("SAMPLE_REG_NO");
            entity.Property(e => e.SentToSap)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("SENT_TO_SAP");
            entity.Property(e => e.TotalCgst)
                .HasColumnType("NUMBER(10,2)")
                .HasColumnName("TOTAL_CGST");
            entity.Property(e => e.TotalIgst)
                .HasColumnType("NUMBER(10,2)")
                .HasColumnName("TOTAL_IGST");
            entity.Property(e => e.TotalSgst)
                .HasColumnType("NUMBER(10,2)")
                .HasColumnName("TOTAL_SGST");
            entity.Property(e => e.TransactionNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("TRANSACTION_NO");
            entity.Property(e => e.UserId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("USER_ID");

            entity.HasOne(d => d.SampleRegNoNavigation).WithMany(p => p.T55LabInvoices)
                .HasPrincipalKey(p => p.SampleRegNo)
                .HasForeignKey(d => d.SampleRegNo)
                .HasConstraintName("FK55_SAMPLE_REG_NO");
        });

        modelBuilder.Entity<T56LabPayment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK_PAYMENT_ID");

            entity.ToTable("T56_LAB_PAYMENTS");

            entity.HasIndex(e => new { e.BankCd, e.ChqNo, e.ChqDt }, "UK56_LAB_PAYMENT").IsUnique();

            entity.Property(e => e.PaymentId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("PAYMENT_ID");
            entity.Property(e => e.Amount)
                .HasColumnType("NUMBER(12,2)")
                .HasColumnName("AMOUNT");
            entity.Property(e => e.BankCd)
                .HasPrecision(3)
                .HasColumnName("BANK_CD");
            entity.Property(e => e.ChqDt)
                .HasColumnType("DATE")
                .HasColumnName("CHQ_DT");
            entity.Property(e => e.ChqNo)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("CHQ_NO");
            entity.Property(e => e.Datetime)
                .HasColumnType("DATE")
                .HasColumnName("DATETIME");
            entity.Property(e => e.LabId)
                .HasPrecision(4)
                .HasColumnName("LAB_ID");
            entity.Property(e => e.PaymentDt)
                .HasColumnType("DATE")
                .HasColumnName("PAYMENT_DT");
            entity.Property(e => e.Remarks)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("REMARKS");
            entity.Property(e => e.UserId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("USER_ID");
        });

        modelBuilder.Entity<T57OngoingContract>(entity =>
        {
            entity.HasKey(e => e.ContractId).HasName("PK_CONTRACTS");

            entity.ToTable("T57_ONGOING_CONTRACTS");

            entity.Property(e => e.ContractId)
                .HasPrecision(8)
                .ValueGeneratedNever()
                .HasColumnName("CONTRACT_ID");
            entity.Property(e => e.ClientName)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("CLIENT_NAME");
            entity.Property(e => e.ContInspFee)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("CONT_INSP_FEE");
            entity.Property(e => e.ContPerFrom)
                .HasColumnType("DATE")
                .HasColumnName("CONT_PER_FROM");
            entity.Property(e => e.ContPerTo)
                .HasColumnType("DATE")
                .HasColumnName("CONT_PER_TO");
            entity.Property(e => e.ContSignDt)
                .HasColumnType("DATE")
                .HasColumnName("CONT_SIGN_DT");
            entity.Property(e => e.ContractCm)
                .HasPrecision(3)
                .HasColumnName("CONTRACT_CM");
            entity.Property(e => e.ContractFee)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("CONTRACT_FEE");
            entity.Property(e => e.ContractFeeNum)
                .HasColumnType("NUMBER(13,2)")
                .HasColumnName("CONTRACT_FEE_NUM");
            entity.Property(e => e.ContractNo)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("CONTRACT_NO");
            entity.Property(e => e.ContractPanalty)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("CONTRACT_PANALTY");
            entity.Property(e => e.ContractSpecialCondn)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("CONTRACT_SPECIAL_CONDN");
            entity.Property(e => e.Createdby)
                .HasPrecision(6)
                .HasColumnName("CREATEDBY");
            entity.Property(e => e.Createddate)
                .HasColumnType("DATE")
                .HasColumnName("CREATEDDATE");
            entity.Property(e => e.Datetime)
                .HasColumnType("DATE")
                .HasColumnName("DATETIME");
            entity.Property(e => e.ExpOr)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("EXP_OR");
            entity.Property(e => e.Isdeleted)
                .HasPrecision(2)
                .HasColumnName("ISDELETED");
            entity.Property(e => e.OfferDt)
                .HasColumnType("DATE")
                .HasColumnName("OFFER_DT");
            entity.Property(e => e.RegionCode)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("REGION_CODE");
            entity.Property(e => e.ScopeOfWork)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("SCOPE_OF_WORK");
            entity.Property(e => e.Status)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("STATUS");
            entity.Property(e => e.Updatedby)
                .HasPrecision(6)
                .HasColumnName("UPDATEDBY");
            entity.Property(e => e.Updateddate)
                .HasColumnType("DATE")
                .HasColumnName("UPDATEDDATE");
            entity.Property(e => e.UserId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("USER_ID");
        });

        modelBuilder.Entity<T58ClientContact>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("T58_CLIENT_CONTACT_PK");

            entity.ToTable("T58_CLIENT_CONTACT");

            entity.HasIndex(e => new { e.VisitDt, e.Client, e.RitesOfficerCd, e.TypeCb }, "PK_CLIENT_CONTACT_NEW").IsUnique();

            entity.Property(e => e.Id)
                .HasPrecision(6)
                .HasDefaultValueSql("\"IBSDEV\".\"T58_CLIENT_CONTACT_SEQ\".\"NEXTVAL\"")
                .HasColumnName("ID");
            entity.Property(e => e.Client)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CLIENT");
            entity.Property(e => e.ClientOfficerName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("CLIENT_OFFICER_NAME");
            entity.Property(e => e.ClientType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CLIENT_TYPE");
            entity.Property(e => e.Createdby)
                .HasPrecision(6)
                .HasColumnName("CREATEDBY");
            entity.Property(e => e.Createddate)
                .HasColumnType("DATE")
                .HasColumnName("CREATEDDATE");
            entity.Property(e => e.Datetime)
                .HasColumnType("DATE")
                .HasColumnName("DATETIME");
            entity.Property(e => e.Designation)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("DESIGNATION");
            entity.Property(e => e.Highlights)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("HIGHLIGHTS");
            entity.Property(e => e.Isdeleted)
                .HasPrecision(2)
                .HasColumnName("ISDELETED");
            entity.Property(e => e.OutAmt)
                .HasColumnType("NUMBER(13,2)")
                .HasColumnName("OUT_AMT");
            entity.Property(e => e.OverallOutcome)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("OVERALL_OUTCOME");
            entity.Property(e => e.RegionCd)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("REGION_CD");
            entity.Property(e => e.RitesOfficerCd)
                .HasPrecision(6)
                .HasColumnName("RITES_OFFICER_CD");
            entity.Property(e => e.TypeCb)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("TYPE_CB");
            entity.Property(e => e.Updatedby)
                .HasPrecision(6)
                .HasColumnName("UPDATEDBY");
            entity.Property(e => e.Updateddate)
                .HasColumnType("DATE")
                .HasColumnName("UPDATEDDATE");
            entity.Property(e => e.UserId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("USER_ID");
            entity.Property(e => e.VisitDt)
                .HasColumnType("DATE")
                .HasColumnName("VISIT_DT");
        });

        modelBuilder.Entity<T59LabExp>(entity =>
        {
            entity.HasKey(e => new { e.LabBillPer, e.RegionCode }).HasName("PKT59_LAB_EXP");

            entity.ToTable("T59_LAB_EXP");

            entity.Property(e => e.LabBillPer)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("LAB_BILL_PER");
            entity.Property(e => e.RegionCode)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("REGION_CODE");
            entity.Property(e => e.Datetime)
                .HasColumnType("DATE")
                .HasColumnName("DATETIME");
            entity.Property(e => e.LabExp)
                .HasColumnType("NUMBER(13,2)")
                .HasColumnName("LAB_EXP");
            entity.Property(e => e.UserId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("USER_ID");
        });

        modelBuilder.Entity<T60IePoiMapping>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("T60_IE_POI_MAPPING");

            entity.Property(e => e.IeCd)
                .HasPrecision(6)
                .HasColumnName("IE_CD");
            entity.Property(e => e.PoiCd)
                .HasPrecision(6)
                .HasColumnName("POI_CD");

            entity.HasOne(d => d.IeCdNavigation).WithMany()
                .HasForeignKey(d => d.IeCd)
                .HasConstraintName("FKT09_IE_CD");

            entity.HasOne(d => d.PoiCdNavigation).WithMany()
                .HasForeignKey(d => d.PoiCd)
                .HasConstraintName("FKT37_POI_CD");
        });

        modelBuilder.Entity<T61ItemMaster>(entity =>
        {
            entity.HasKey(e => e.ItemCd).HasName("PK_ITEM_CD");

            entity.ToTable("T61_ITEM_MASTER");

            entity.Property(e => e.ItemCd)
                .HasMaxLength(7)
                .IsUnicode(false)
                .HasColumnName("ITEM_CD");
            entity.Property(e => e.Checksheet)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("CHECKSHEET");
            entity.Property(e => e.Cm)
                .HasPrecision(3)
                .HasColumnName("CM");
            entity.Property(e => e.Createdby)
                .HasPrecision(6)
                .HasColumnName("CREATEDBY");
            entity.Property(e => e.Createddate)
                .HasColumnType("DATE")
                .HasColumnName("CREATEDDATE");
            entity.Property(e => e.CreationRevDt)
                .HasColumnType("DATE")
                .HasColumnName("CREATION_REV_DT");
            entity.Property(e => e.Datetime)
                .HasColumnType("DATE")
                .HasColumnName("DATETIME");
            entity.Property(e => e.Department)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("DEPARTMENT");
            entity.Property(e => e.Ie)
                .HasPrecision(4)
                .HasColumnName("IE");
            entity.Property(e => e.Isdeleted)
                .HasPrecision(2)
                .HasColumnName("ISDELETED");
            entity.Property(e => e.ItemDesc)
                .HasMaxLength(400)
                .IsUnicode(false)
                .HasColumnName("ITEM_DESC");
            entity.Property(e => e.TimeForInsp)
                .HasPrecision(2)
                .HasColumnName("TIME_FOR_INSP");
            entity.Property(e => e.Updatedby)
                .HasPrecision(6)
                .HasColumnName("UPDATEDBY");
            entity.Property(e => e.Updateddate)
                .HasColumnType("DATE")
                .HasColumnName("UPDATEDDATE");
            entity.Property(e => e.UserId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("USER_ID");
        });

        modelBuilder.Entity<T62MasterItemPlno>(entity =>
        {
            entity.HasKey(e => e.PlNo).HasName("PK_PL_NO");

            entity.ToTable("T62_MASTER_ITEM_PLNO");

            entity.HasIndex(e => new { e.ItemCd, e.PlNo }, "UK_ITEM_PL_NO").IsUnique();

            entity.Property(e => e.PlNo)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("PL_NO");
            entity.Property(e => e.Datetime)
                .HasColumnType("DATE")
                .HasColumnName("DATETIME");
            entity.Property(e => e.ItemCd)
                .HasMaxLength(7)
                .IsUnicode(false)
                .HasColumnName("ITEM_CD");
            entity.Property(e => e.UserId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("USER_ID");
        });

        modelBuilder.Entity<T63Exp>(entity =>
        {
            entity.HasKey(e => new { e.ExpPer, e.RegionCode }).HasName("PKT63_EXP");

            entity.ToTable("T63_EXP");

            entity.Property(e => e.ExpPer)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("EXP_PER");
            entity.Property(e => e.RegionCode)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("REGION_CODE");
            entity.Property(e => e.Datetime)
                .HasColumnType("DATE")
                .HasColumnName("DATETIME");
            entity.Property(e => e.ExpAmt)
                .HasColumnType("NUMBER(13,2)")
                .HasColumnName("EXP_AMT");
            entity.Property(e => e.TaxAmt)
                .HasColumnType("NUMBER(11,2)")
                .HasColumnName("TAX_AMT");
            entity.Property(e => e.UserId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("USER_ID");
        });

        modelBuilder.Entity<T64TestCategory>(entity =>
        {
            entity.HasKey(e => e.TestCategoryCd).HasName("PK_TEST_CATEGORY_CD");

            entity.ToTable("T64_TEST_CATEGORY");

            entity.Property(e => e.TestCategoryCd)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("TEST_CATEGORY_CD");
            entity.Property(e => e.TestCategoryDesc)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("TEST_CATEGORY_DESC");
        });

        modelBuilder.Entity<T65LaboratoryMaster>(entity =>
        {
            entity.HasKey(e => e.LabId).HasName("T65_LABORATORY_MASTER_PK");

            entity.ToTable("T65_LABORATORY_MASTER");

            entity.Property(e => e.LabId)
                .HasPrecision(6)
                .HasDefaultValueSql("\"IBSDEV\".\"T65_LABORATORY_MASTER_SEQ\".\"NEXTVAL\"")
                .HasColumnName("LAB_ID");
            entity.Property(e => e.Datetime)
                .HasColumnType("DATE")
                .HasColumnName("DATETIME");
            entity.Property(e => e.LabAddress)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("LAB_ADDRESS");
            entity.Property(e => e.LabApproval)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("LAB_APPROVAL");
            entity.Property(e => e.LabApprovalFr)
                .HasColumnType("DATE")
                .HasColumnName("LAB_APPROVAL_FR");
            entity.Property(e => e.LabApprovalTo)
                .HasColumnType("DATE")
                .HasColumnName("LAB_APPROVAL_TO");
            entity.Property(e => e.LabCity)
                .HasPrecision(6)
                .HasColumnName("LAB_CITY");
            entity.Property(e => e.LabContactPer)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("LAB_CONTACT_PER");
            entity.Property(e => e.LabContactTel)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("LAB_CONTACT_TEL");
            entity.Property(e => e.LabEmail)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("LAB_EMAIL");
            entity.Property(e => e.LabName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("LAB_NAME");
            entity.Property(e => e.UserId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("USER_ID");
        });

        modelBuilder.Entity<T65LaboratoryMasterBak>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("T65_LABORATORY_MASTER_Bak");

            entity.Property(e => e.Datetime)
                .HasColumnType("DATE")
                .HasColumnName("DATETIME");
            entity.Property(e => e.LabAddress)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("LAB_ADDRESS");
            entity.Property(e => e.LabApproval)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("LAB_APPROVAL");
            entity.Property(e => e.LabApprovalFr)
                .HasColumnType("DATE")
                .HasColumnName("LAB_APPROVAL_FR");
            entity.Property(e => e.LabApprovalTo)
                .HasColumnType("DATE")
                .HasColumnName("LAB_APPROVAL_TO");
            entity.Property(e => e.LabCity)
                .HasPrecision(6)
                .HasColumnName("LAB_CITY");
            entity.Property(e => e.LabContactPer)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("LAB_CONTACT_PER");
            entity.Property(e => e.LabContactTel)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("LAB_CONTACT_TEL");
            entity.Property(e => e.LabEmail)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("LAB_EMAIL");
            entity.Property(e => e.LabId)
                .HasPrecision(6)
                .HasDefaultValueSql("\"IBSDEV\".\"T65_LABORATORY_MASTER_SEQ\".\"NEXTVAL\"")
                .HasColumnName("LAB_ID");
            entity.Property(e => e.LabName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("LAB_NAME");
            entity.Property(e => e.UserId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("USER_ID");
        });

        modelBuilder.Entity<T65LaboratoryMasterBak1>(entity =>
        {
            entity.HasKey(e => e.LabId).HasName("T65_LABORATORY_MASTER_BAK_PK");

            entity.ToTable("T65_LABORATORY_MASTER_BAK");

            entity.Property(e => e.LabId)
                .HasPrecision(6)
                .HasDefaultValueSql("\"IBSDEV\".\"T65_LABORATORY_MASTER_SEQ\".\"NEXTVAL\"")
                .HasColumnName("LAB_ID");
            entity.Property(e => e.Datetime)
                .HasColumnType("DATE")
                .HasColumnName("DATETIME");
            entity.Property(e => e.LabAddress)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("LAB_ADDRESS");
            entity.Property(e => e.LabApproval)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("LAB_APPROVAL");
            entity.Property(e => e.LabApprovalFr)
                .HasColumnType("DATE")
                .HasColumnName("LAB_APPROVAL_FR");
            entity.Property(e => e.LabApprovalTo)
                .HasColumnType("DATE")
                .HasColumnName("LAB_APPROVAL_TO");
            entity.Property(e => e.LabCity)
                .HasPrecision(6)
                .HasColumnName("LAB_CITY");
            entity.Property(e => e.LabContactPer)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("LAB_CONTACT_PER");
            entity.Property(e => e.LabContactTel)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("LAB_CONTACT_TEL");
            entity.Property(e => e.LabEmail)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("LAB_EMAIL");
            entity.Property(e => e.LabName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("LAB_NAME");
            entity.Property(e => e.UserId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("USER_ID");
        });

        modelBuilder.Entity<T66TechRef>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("T66_TECH_REF");

            entity.Property(e => e.Datetime)
                .HasColumnType("DATE")
                .HasColumnName("DATETIME");
            entity.Property(e => e.RegionCd)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("REGION_CD");
            entity.Property(e => e.TechCmCd)
                .HasPrecision(3)
                .HasColumnName("TECH_CM_CD");
            entity.Property(e => e.TechContent)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("TECH_CONTENT");
            entity.Property(e => e.TechDate)
                .HasColumnType("DATE")
                .HasColumnName("TECH_DATE");
            entity.Property(e => e.TechId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("TECH_ID");
            entity.Property(e => e.TechIeCd)
                .HasPrecision(4)
                .HasColumnName("TECH_IE_CD");
            entity.Property(e => e.TechItemDes)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("TECH_ITEM_DES");
            entity.Property(e => e.TechLetterNo)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("TECH_LETTER_NO");
            entity.Property(e => e.TechRefMade)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("TECH_REF_MADE");
            entity.Property(e => e.TechSpecDrg)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("TECH_SPEC_DRG");
            entity.Property(e => e.UserId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("USER_ID");
        });

        modelBuilder.Entity<T67Highlight>(entity =>
        {
            entity.HasKey(e => new { e.RegionCode, e.HighDt });

            entity.ToTable("T67_HIGHLIGHTS");

            entity.Property(e => e.RegionCode)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("REGION_CODE");
            entity.Property(e => e.HighDt)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("HIGH_DT");
            entity.Property(e => e.Datetime)
                .HasColumnType("DATE")
                .HasColumnName("DATETIME");
            entity.Property(e => e.HightText)
                .HasMaxLength(3000)
                .IsUnicode(false)
                .HasColumnName("HIGHT_TEXT");
            entity.Property(e => e.UserId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("USER_ID");
        });

        modelBuilder.Entity<T68SealingPatternCode>(entity =>
        {
            entity.HasKey(e => e.SealingPatternCd).HasName("PK_SEALING_PATTERN_CD");

            entity.ToTable("T68_SEALING_PATTERN_CODES");

            entity.Property(e => e.SealingPatternCd)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("SEALING_PATTERN_CD");
            entity.Property(e => e.SealingPatternDesc)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("SEALING_PATTERN_DESC");
        });

        modelBuilder.Entity<T69NcCode>(entity =>
        {
            entity.HasKey(e => e.NcCd);

            entity.ToTable("T69_NC_CODES");

            entity.Property(e => e.NcCd)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("NC_CD");
            entity.Property(e => e.NcClass)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("NC_CLASS");
            entity.Property(e => e.NcDesc)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("NC_DESC");
        });

        modelBuilder.Entity<T70UnregisteredCall>(entity =>
        {
            entity.HasKey(e => e.IeCd).HasName("PK70_IECD");

            entity.ToTable("T70_UNREGISTERED_CALLS");

            entity.Property(e => e.IeCd)
                .HasPrecision(6)
                .ValueGeneratedNever()
                .HasColumnName("IE_CD");
            entity.Property(e => e.Region)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("REGION");
            entity.Property(e => e.UnregCalls)
                .HasPrecision(4)
                .HasColumnName("UNREG_CALLS");
            entity.Property(e => e.YrMth)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("YR_MTH");

            entity.HasOne(d => d.IeCdNavigation).WithOne(p => p.T70UnregisteredCall)
                .HasForeignKey<T70UnregisteredCall>(d => d.IeCd)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK70_IECD");
        });

        modelBuilder.Entity<T71RcfBill>(entity =>
        {
            entity.HasKey(e => e.BillNo).HasName("PK71_BILL_NO");

            entity.ToTable("T71_RCF_BILLS");

            entity.Property(e => e.BillNo)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("BILL_NO");
            entity.Property(e => e.BillDt)
                .HasColumnType("DATE")
                .HasColumnName("BILL_DT");
            entity.Property(e => e.InvoiceNo)
                .HasPrecision(5)
                .HasColumnName("INVOICE_NO");
            entity.Property(e => e.PoNo)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("PO_NO");
            entity.Property(e => e.PoSeries)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("PO_SERIES");
            entity.Property(e => e.Region)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("REGION");

            entity.HasOne(d => d.BillNoNavigation).WithOne(p => p.T71RcfBill)
                .HasForeignKey<T71RcfBill>(d => d.BillNo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK71_BILL_NO");
        });

        modelBuilder.Entity<T71RcfBills16apr2010>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("T71_RCF_BILLS_16APR2010");

            entity.Property(e => e.BillDt)
                .HasColumnType("DATE")
                .HasColumnName("BILL_DT");
            entity.Property(e => e.BillNo)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("BILL_NO");
            entity.Property(e => e.InvoiceNo)
                .HasPrecision(5)
                .HasColumnName("INVOICE_NO");
            entity.Property(e => e.PoNo)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("PO_NO");
            entity.Property(e => e.PoSeries)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("PO_SERIES");
            entity.Property(e => e.Region)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("REGION");
        });

        modelBuilder.Entity<T72IeMessage>(entity =>
        {
            entity.HasKey(e => new { e.MessageId, e.RegionCode }).HasName("PK72_MESSAGE_ID");

            entity.ToTable("T72_IE_MESSAGES");

            entity.Property(e => e.MessageId)
                .HasPrecision(6)
                .HasColumnName("MESSAGE_ID");
            entity.Property(e => e.RegionCode)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("REGION_CODE");
            entity.Property(e => e.Createdby)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CREATEDBY");
            entity.Property(e => e.Createddate)
                .HasColumnType("TIMESTAMP(6) WITH TIME ZONE")
                .HasColumnName("CREATEDDATE");
            entity.Property(e => e.Datetime)
                .HasColumnType("DATE")
                .HasColumnName("DATETIME");
            entity.Property(e => e.Isdeleted)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ISDELETED");
            entity.Property(e => e.LetterDt)
                .HasColumnType("DATE")
                .HasColumnName("LETTER_DT");
            entity.Property(e => e.LetterNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("LETTER_NO");
            entity.Property(e => e.Message)
                .HasMaxLength(800)
                .IsUnicode(false)
                .HasColumnName("MESSAGE");
            entity.Property(e => e.MessageDt)
                .HasColumnType("DATE")
                .HasColumnName("MESSAGE_DT");
            entity.Property(e => e.Updatedby)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("UPDATEDBY");
            entity.Property(e => e.Updateddate)
                .HasColumnType("TIMESTAMP(6) WITH TIME ZONE")
                .HasColumnName("UPDATEDDATE");
            entity.Property(e => e.UserId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("USER_ID");
        });

        modelBuilder.Entity<T73PayingWindow>(entity =>
        {
            entity.HasKey(e => e.PayWindowId).HasName("PK73_WINDOW_ID");

            entity.ToTable("T73_PAYING_WINDOW");

            entity.Property(e => e.PayWindowId)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("PAY_WINDOW_ID");
            entity.Property(e => e.PayWindowDesc)
                .HasMaxLength(55)
                .IsUnicode(false)
                .HasColumnName("PAY_WINDOW_DESC");
        });

        modelBuilder.Entity<T74ChecksheetCatalog>(entity =>
        {
            entity.HasKey(e => e.ChkSheetId).HasName("PK74_CHK_SHEET_ID");

            entity.ToTable("T74_CHECKSHEET_CATALOG");

            entity.Property(e => e.ChkSheetId)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CHK_SHEET_ID");
            entity.Property(e => e.ChkSheetName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CHK_SHEET_NAME");
            entity.Property(e => e.Datetime)
                .HasColumnType("DATE")
                .HasColumnName("DATETIME");
            entity.Property(e => e.Discipline)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("DISCIPLINE");
            entity.Property(e => e.DocumentNo)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("DOCUMENT_NO");
            entity.Property(e => e.FileExt)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("FILE_EXT");
            entity.Property(e => e.IssueDt)
                .HasColumnType("DATE")
                .HasColumnName("ISSUE_DT");
            entity.Property(e => e.UserId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("USER_ID");
        });

        modelBuilder.Entity<T74DocumentType>(entity =>
        {
            entity.HasKey(e => e.DocType).HasName("PK_DOC_TYPE");

            entity.ToTable("T74_DOCUMENT_TYPES");

            entity.Property(e => e.DocType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("DOC_TYPE");
            entity.Property(e => e.DocTypeDesc)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("DOC_TYPE_DESC");
        });

        modelBuilder.Entity<T75DocSubType>(entity =>
        {
            entity.HasKey(e => new { e.DocType, e.DocSubType });

            entity.ToTable("T75_DOC_SUB_TYPES");

            entity.Property(e => e.DocType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("DOC_TYPE");
            entity.Property(e => e.DocSubType)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("DOC_SUB_TYPE");
            entity.Property(e => e.DocSubTypeDesc)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("DOC_SUB_TYPE_DESC");

            entity.HasOne(d => d.DocTypeNavigation).WithMany(p => p.T75DocSubTypes)
                .HasForeignKey(d => d.DocType)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DOC_TYPE");
        });

        modelBuilder.Entity<T76DocumentCatalog>(entity =>
        {
            entity.HasKey(e => e.FileId).HasName("PK76_FILE_ID");

            entity.ToTable("T76_DOCUMENT_CATALOG");

            entity.Property(e => e.FileId)
                .HasMaxLength(7)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("FILE_ID");
            entity.Property(e => e.Datetime)
                .HasColumnType("DATE")
                .HasColumnName("DATETIME");
            entity.Property(e => e.DocSubType)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("DOC_SUB_TYPE");
            entity.Property(e => e.DocType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("DOC_TYPE");
            entity.Property(e => e.DocumentName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("DOCUMENT_NAME");
            entity.Property(e => e.DocumentNo)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("DOCUMENT_NO");
            entity.Property(e => e.FileExt)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("FILE_EXT");
            entity.Property(e => e.IssueDt)
                .HasColumnType("DATE")
                .HasColumnName("ISSUE_DT");
            entity.Property(e => e.Region)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("REGION");
            entity.Property(e => e.UserId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("USER_ID");
        });

        modelBuilder.Entity<T77IcfBill>(entity =>
        {
            entity.HasKey(e => e.BillNo).HasName("PK77_BILL_NO");

            entity.ToTable("T77_ICF_BILLS");

            entity.Property(e => e.BillNo)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("BILL_NO");
            entity.Property(e => e.BillAmount)
                .HasColumnType("NUMBER(13,2)")
                .HasColumnName("BILL_AMOUNT");
            entity.Property(e => e.BillDt)
                .HasColumnType("DATE")
                .HasColumnName("BILL_DT");
            entity.Property(e => e.BkNo)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("BK_NO");
            entity.Property(e => e.CallInstalmentNo)
                .HasMaxLength(47)
                .IsUnicode(false)
                .HasColumnName("CALL_INSTALMENT_NO");
            entity.Property(e => e.IcDt)
                .HasColumnType("DATE")
                .HasColumnName("IC_DT");
            entity.Property(e => e.InspFee)
                .HasColumnType("NUMBER(13,2)")
                .HasColumnName("INSP_FEE");
            entity.Property(e => e.InvoiceNo)
                .HasPrecision(5)
                .HasColumnName("INVOICE_NO");
            entity.Property(e => e.KrishiKalyanCess)
                .HasColumnType("NUMBER(10,2)")
                .HasColumnName("KRISHI_KALYAN_CESS");
            entity.Property(e => e.L4noPo)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("L4NO_PO");
            entity.Property(e => e.MaterialValue)
                .HasColumnType("NUMBER(15,2)")
                .HasColumnName("MATERIAL_VALUE");
            entity.Property(e => e.PoDt)
                .HasColumnType("DATE")
                .HasColumnName("PO_DT");
            entity.Property(e => e.PoNo)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("PO_NO");
            entity.Property(e => e.PoSeries)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("PO_SERIES");
            entity.Property(e => e.Region)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("REGION");
            entity.Property(e => e.ServiceTax)
                .HasColumnType("NUMBER(11,2)")
                .HasColumnName("SERVICE_TAX");
            entity.Property(e => e.SetNo)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("SET_NO");
            entity.Property(e => e.SwachhBharatCess)
                .HasColumnType("NUMBER(10,2)")
                .HasColumnName("SWACHH_BHARAT_CESS");

            entity.HasOne(d => d.BillNoNavigation).WithOne(p => p.T77IcfBill)
                .HasForeignKey<T77IcfBill>(d => d.BillNo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK77_BILL_NO");
        });

        modelBuilder.Entity<T78CentralQoi>(entity =>
        {
            entity.HasKey(e => new { e.Client, e.QtyDate }).HasName("T78_CENTRAL_QOI");

            entity.ToTable("T78_CENTRAL_QOI");

            entity.Property(e => e.Client)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CLIENT");
            entity.Property(e => e.QtyDate)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("QTY_DATE");
            entity.Property(e => e.Datetime)
                .HasColumnType("DATE")
                .HasColumnName("DATETIME");
            entity.Property(e => e.NoOfIcIssued)
                .HasPrecision(13)
                .HasColumnName("NO_OF_IC_ISSUED");
            entity.Property(e => e.RegionCode)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("REGION_CODE");
            entity.Property(e => e.TotalQtyDispatched)
                .HasPrecision(13)
                .HasColumnName("TOTAL_QTY_DISPATCHED");
            entity.Property(e => e.UserId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("USER_ID");
        });

        modelBuilder.Entity<T79CentralQoinsp>(entity =>
        {
            entity.HasKey(e => new { e.Client, e.QoiDate, e.Weight, e.QoiLength }).HasName("T79_CENTRAL_QOINSP");

            entity.ToTable("T79_CENTRAL_QOINSP");

            entity.Property(e => e.Client)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CLIENT");
            entity.Property(e => e.QoiDate)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("QOI_DATE");
            entity.Property(e => e.Weight)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("WEIGHT");
            entity.Property(e => e.QoiLength)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("QOI_LENGTH");
            entity.Property(e => e.Accepted)
                .HasPrecision(13)
                .HasColumnName("ACCEPTED");
            entity.Property(e => e.Datetime)
                .HasColumnType("DATE")
                .HasColumnName("DATETIME");
            entity.Property(e => e.Region)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("REGION");
            entity.Property(e => e.Rejected)
                .HasPrecision(13)
                .HasColumnName("REJECTED");
            entity.Property(e => e.UserId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("USER_ID");
        });

        modelBuilder.Entity<T80PoMaster>(entity =>
        {
            entity.HasKey(e => e.CaseNo).HasName("PK80_PO_NO");

            entity.ToTable("T80_PO_MASTER");

            entity.Property(e => e.CaseNo)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CASE_NO");
            entity.Property(e => e.Createdby)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CREATEDBY");
            entity.Property(e => e.Createddate)
                .HasColumnType("TIMESTAMP(6) WITH TIME ZONE")
                .HasColumnName("CREATEDDATE");
            entity.Property(e => e.Datetime)
                .HasColumnType("DATE")
                .HasColumnName("DATETIME");
            entity.Property(e => e.Isdeleted)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ISDELETED");
            entity.Property(e => e.PoDt)
                .HasColumnType("DATE")
                .HasColumnName("PO_DT");
            entity.Property(e => e.PoNo)
                .HasMaxLength(75)
                .IsUnicode(false)
                .HasColumnName("PO_NO");
            entity.Property(e => e.PoOrLetter)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("PO_OR_LETTER");
            entity.Property(e => e.PoiCd)
                .HasPrecision(6)
                .HasColumnName("POI_CD");
            entity.Property(e => e.Purchaser)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("PURCHASER");
            entity.Property(e => e.PurchaserCd)
                .HasPrecision(8)
                .HasColumnName("PURCHASER_CD");
            entity.Property(e => e.RealCaseNo)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("REAL_CASE_NO");
            entity.Property(e => e.RecvDt)
                .HasColumnType("DATE")
                .HasColumnName("RECV_DT");
            entity.Property(e => e.RegionCode)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("REGION_CODE");
            entity.Property(e => e.Remarks)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("REMARKS");
            entity.Property(e => e.RlyCd)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("RLY_CD");
            entity.Property(e => e.RlyCdDesc)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("RLY_CD_DESC");
            entity.Property(e => e.RlyNonrly)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("RLY_NONRLY");
            entity.Property(e => e.StockNonstock)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("STOCK_NONSTOCK");
            entity.Property(e => e.Updatedby)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("UPDATEDBY");
            entity.Property(e => e.Updateddate)
                .HasColumnType("TIMESTAMP(6) WITH TIME ZONE")
                .HasColumnName("UPDATEDDATE");
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("USER_ID");
            entity.Property(e => e.VendCd)
                .HasPrecision(6)
                .HasColumnName("VEND_CD");

            entity.HasOne(d => d.PurchaserCdNavigation).WithMany(p => p.T80PoMasters)
                .HasForeignKey(d => d.PurchaserCd)
                .HasConstraintName("FK80_PURCHASER_CD");

            entity.HasOne(d => d.RegionCodeNavigation).WithMany(p => p.T80PoMasters)
                .HasForeignKey(d => d.RegionCode)
                .HasConstraintName("FK80_REGION_CODE");

            entity.HasOne(d => d.VendCdNavigation).WithMany(p => p.T80PoMasters)
                .HasForeignKey(d => d.VendCd)
                .HasConstraintName("FK80_VEND_CD");
        });

        modelBuilder.Entity<T81CrRej>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("T81_CR_REJ");

            entity.Property(e => e.CaseNo)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("CASE_NO");
            entity.Property(e => e.Conclusion)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("CONCLUSION");
            entity.Property(e => e.Consignee)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("CONSIGNEE");
            entity.Property(e => e.Datetime)
                .HasColumnType("DATE")
                .HasColumnName("DATETIME");
            entity.Property(e => e.DesCom)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("DES_COM");
            entity.Property(e => e.Region)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("REGION");
            entity.Property(e => e.RejDt)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("REJ_DT");
            entity.Property(e => e.UserId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("USER_ID");
        });

        modelBuilder.Entity<T82PoDetail>(entity =>
        {
            entity.HasKey(e => new { e.CaseNo, e.ItemSrno }).HasName("PK82_PO_DETAILS");

            entity.ToTable("T82_PO_DETAIL");

            entity.Property(e => e.CaseNo)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CASE_NO");
            entity.Property(e => e.ItemSrno)
                .HasPrecision(4)
                .HasColumnName("ITEM_SRNO");
            entity.Property(e => e.BasicValue)
                .HasColumnType("NUMBER(13,2)")
                .HasColumnName("BASIC_VALUE");
            entity.Property(e => e.Bpo)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("BPO");
            entity.Property(e => e.BpoCd)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("BPO_CD");
            entity.Property(e => e.Consignee)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("CONSIGNEE");
            entity.Property(e => e.ConsigneeCd)
                .HasPrecision(8)
                .HasColumnName("CONSIGNEE_CD");
            entity.Property(e => e.Datetime)
                .HasColumnType("DATE")
                .HasColumnName("DATETIME");
            entity.Property(e => e.DelvDt)
                .HasColumnType("DATE")
                .HasColumnName("DELV_DT");
            entity.Property(e => e.Discount)
                .HasColumnType("NUMBER(10,2)")
                .HasColumnName("DISCOUNT");
            entity.Property(e => e.DiscountPer)
                .HasColumnType("NUMBER(10,2)")
                .HasColumnName("DISCOUNT_PER");
            entity.Property(e => e.DiscountType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("DISCOUNT_TYPE");
            entity.Property(e => e.Excise)
                .HasColumnType("NUMBER(10,2)")
                .HasColumnName("EXCISE");
            entity.Property(e => e.ExcisePer)
                .HasColumnType("NUMBER(10,2)")
                .HasColumnName("EXCISE_PER");
            entity.Property(e => e.ExciseType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("EXCISE_TYPE");
            entity.Property(e => e.ExtDelvDt)
                .HasColumnType("DATE")
                .HasColumnName("EXT_DELV_DT");
            entity.Property(e => e.ItemCd)
                .HasMaxLength(7)
                .IsUnicode(false)
                .HasColumnName("ITEM_CD");
            entity.Property(e => e.ItemDesc)
                .HasMaxLength(400)
                .IsUnicode(false)
                .HasColumnName("ITEM_DESC");
            entity.Property(e => e.OtChargePer)
                .HasColumnType("NUMBER(10,2)")
                .HasColumnName("OT_CHARGE_PER");
            entity.Property(e => e.OtChargeType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("OT_CHARGE_TYPE");
            entity.Property(e => e.OtherCharges)
                .HasColumnType("NUMBER(10,2)")
                .HasColumnName("OTHER_CHARGES");
            entity.Property(e => e.PlNo)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("PL_NO");
            entity.Property(e => e.Qty)
                .HasColumnType("NUMBER(11,3)")
                .HasColumnName("QTY");
            entity.Property(e => e.Rate)
                .HasColumnType("NUMBER(11,2)")
                .HasColumnName("RATE");
            entity.Property(e => e.SalesTax)
                .HasColumnType("NUMBER(10,2)")
                .HasColumnName("SALES_TAX");
            entity.Property(e => e.SalesTaxPer)
                .HasColumnType("NUMBER(4,2)")
                .HasColumnName("SALES_TAX_PER");
            entity.Property(e => e.UomCd)
                .HasPrecision(3)
                .HasColumnName("UOM_CD");
            entity.Property(e => e.UserId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("USER_ID");
            entity.Property(e => e.Value)
                .HasColumnType("NUMBER(15,2)")
                .HasColumnName("VALUE");

            entity.HasOne(d => d.CaseNoNavigation).WithMany(p => p.T82PoDetails)
                .HasForeignKey(d => d.CaseNo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK82_CASE_NO");

            entity.HasOne(d => d.UomCdNavigation).WithMany(p => p.T82PoDetails)
                .HasForeignKey(d => d.UomCd)
                .HasConstraintName("FK82_UOM_CD");
        });

        modelBuilder.Entity<T83BeTarget>(entity =>
        {
            entity.HasKey(e => new { e.BePer, e.RegionCode }).HasName("PKT83_BE_TARGET");

            entity.ToTable("T83_BE_TARGET");

            entity.Property(e => e.BePer)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("BE_PER");
            entity.Property(e => e.RegionCode)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("REGION_CODE");
            entity.Property(e => e.BTarget)
                .HasColumnType("NUMBER(13,2)")
                .HasColumnName("B_TARGET");
            entity.Property(e => e.Datetime)
                .HasColumnType("DATE")
                .HasColumnName("DATETIME");
            entity.Property(e => e.ETarget)
                .HasColumnType("NUMBER(13,2)")
                .HasColumnName("E_TARGET");
            entity.Property(e => e.ExTarget)
                .HasColumnType("NUMBER(13,2)")
                .HasColumnName("EX_TARGET");
            entity.Property(e => e.UserId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("USER_ID");
        });

        modelBuilder.Entity<T84OutsLy>(entity =>
        {
            entity.HasKey(e => new { e.LyPer, e.RegionCode }).HasName("PKT84_OUTS_LY");

            entity.ToTable("T84_OUTS_LY");

            entity.Property(e => e.LyPer)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("LY_PER");
            entity.Property(e => e.RegionCode)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("REGION_CODE");
            entity.Property(e => e.Datetime)
                .HasColumnType("DATE")
                .HasColumnName("DATETIME");
            entity.Property(e => e.LyOuts)
                .HasColumnType("NUMBER(13,2)")
                .HasColumnName("LY_OUTS");
            entity.Property(e => e.UserId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("USER_ID");
        });

        modelBuilder.Entity<T85BillingAdjustementPcdo>(entity =>
        {
            entity.HasKey(e => new { e.RegionCode, e.AdjusmentYrMth }).HasName("PK_T85_ADJUSTMENTS");

            entity.ToTable("T85_BILLING_ADJUSTEMENT_PCDO");

            entity.Property(e => e.RegionCode)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("REGION_CODE");
            entity.Property(e => e.AdjusmentYrMth)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("ADJUSMENT_YR_MTH");
            entity.Property(e => e.AdjustmentAmt)
                .HasColumnType("NUMBER(13,2)")
                .HasColumnName("ADJUSTMENT_AMT");
            entity.Property(e => e.Datetime)
                .HasColumnType("DATE")
                .HasColumnName("DATETIME");
            entity.Property(e => e.Remarks)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("REMARKS");
            entity.Property(e => e.UserId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("USER_ID");
        });

        modelBuilder.Entity<T86LabInvoiceDetail>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("T86_LAB_INVOICE_DETAILS");

            entity.Property(e => e.Cgst)
                .HasColumnType("NUMBER(10,2)")
                .HasColumnName("CGST");
            entity.Property(e => e.Igst)
                .HasColumnType("NUMBER(10,2)")
                .HasColumnName("IGST");
            entity.Property(e => e.InvoiceNo)
                .HasMaxLength(14)
                .IsUnicode(false)
                .HasColumnName("INVOICE_NO");
            entity.Property(e => e.ItemDesc)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("ITEM_DESC");
            entity.Property(e => e.ItemSrno)
                .HasPrecision(2)
                .HasColumnName("ITEM_SRNO");
            entity.Property(e => e.Qty)
                .HasColumnType("NUMBER(8,2)")
                .HasColumnName("QTY");
            entity.Property(e => e.Rate)
                .HasColumnType("NUMBER(10,2)")
                .HasColumnName("RATE");
            entity.Property(e => e.Sgst)
                .HasColumnType("NUMBER(10,2)")
                .HasColumnName("SGST");
            entity.Property(e => e.TestingCharges)
                .HasColumnType("NUMBER(11,2)")
                .HasColumnName("TESTING_CHARGES");

            entity.HasOne(d => d.InvoiceNoNavigation).WithMany()
                .HasForeignKey(d => d.InvoiceNo)
                .HasConstraintName("FK_LAB_INVOICE_NO");
        });

        modelBuilder.Entity<T87BillControl>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("T87_BILL_CONTROL");

            entity.Property(e => e.MinBillDt)
                .HasColumnType("DATE")
                .HasColumnName("MIN_BILL_DT");
        });

        modelBuilder.Entity<T88SapBilling>(entity =>
        {
            entity.HasKey(e => new { e.SapBillPer, e.RegionCode }).HasName("PKT88_SAP_BILLING");

            entity.ToTable("T88_SAP_BILLING");

            entity.Property(e => e.SapBillPer)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("SAP_BILL_PER");
            entity.Property(e => e.RegionCode)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("REGION_CODE");
            entity.Property(e => e.Cgst)
                .HasColumnType("NUMBER(10,2)")
                .HasColumnName("CGST");
            entity.Property(e => e.Datetime)
                .HasColumnType("DATE")
                .HasColumnName("DATETIME");
            entity.Property(e => e.Igst)
                .HasColumnType("NUMBER(10,2)")
                .HasColumnName("IGST");
            entity.Property(e => e.InspFee)
                .HasColumnType("NUMBER(13,2)")
                .HasColumnName("INSP_FEE");
            entity.Property(e => e.Sgst)
                .HasColumnType("NUMBER(10,2)")
                .HasColumnName("SGST");
            entity.Property(e => e.UserId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("USER_ID");
        });

        modelBuilder.Entity<T89Gst>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("T89_GST");

            entity.Property(e => e.CgstRate)
                .HasColumnType("NUMBER(5,2)")
                .HasColumnName("CGST_RATE");
            entity.Property(e => e.DtFrom)
                .HasColumnType("DATE")
                .HasColumnName("DT_FROM");
            entity.Property(e => e.DtTo)
                .HasColumnType("DATE")
                .HasColumnName("DT_TO");
            entity.Property(e => e.IgstRate)
                .HasColumnType("NUMBER(5,2)")
                .HasColumnName("IGST_RATE");
            entity.Property(e => e.SgstRate)
                .HasColumnType("NUMBER(5,2)")
                .HasColumnName("SGST_RATE");
        });

        modelBuilder.Entity<T90RlyDesignation>(entity =>
        {
            entity.HasKey(e => e.RlyDesigCd).HasName("PK_RLY_DESIG_CD");

            entity.ToTable("T90_RLY_DESIGNATION");

            entity.Property(e => e.RlyDesigCd)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("RLY_DESIG_CD");
            entity.Property(e => e.Createdby)
                .HasPrecision(6)
                .HasColumnName("CREATEDBY");
            entity.Property(e => e.Createddate)
                .HasColumnType("DATE")
                .HasColumnName("CREATEDDATE");
            entity.Property(e => e.Datetime)
                .HasColumnType("DATE")
                .HasColumnName("DATETIME");
            entity.Property(e => e.Isdeleted)
                .HasPrecision(2)
                .HasColumnName("ISDELETED");
            entity.Property(e => e.RlyDesigDesc)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("RLY_DESIG_DESC");
            entity.Property(e => e.Updatedby)
                .HasPrecision(6)
                .HasColumnName("UPDATEDBY");
            entity.Property(e => e.Updateddate)
                .HasColumnType("DATE")
                .HasColumnName("UPDATEDDATE");
            entity.Property(e => e.UserId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("USER_ID");
        });

        modelBuilder.Entity<T91Railway>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("T91_RAILWAYS_PK");

            entity.ToTable("T91_RAILWAYS");

            entity.HasIndex(e => e.RlyCd, "PK_RLY_CD").IsUnique();

            entity.Property(e => e.Id)
                .HasPrecision(6)
                .HasDefaultValueSql("\"IBSDEV\".\"T91_RAILWAYS_SEQ\".\"NEXTVAL\"")
                .HasColumnName("ID");
            entity.Property(e => e.Createdby)
                .HasPrecision(6)
                .HasColumnName("CREATEDBY");
            entity.Property(e => e.Createddate)
                .HasColumnType("DATE")
                .HasColumnName("CREATEDDATE");
            entity.Property(e => e.Datetime)
                .HasColumnType("DATE")
                .HasColumnName("DATETIME");
            entity.Property(e => e.HeadQuarter)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("HEAD_QUARTER");
            entity.Property(e => e.ImmsRlyCd)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("IMMS_RLY_CD");
            entity.Property(e => e.Isdeleted)
                .HasPrecision(2)
                .HasColumnName("ISDELETED");
            entity.Property(e => e.Railway)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("RAILWAY");
            entity.Property(e => e.RlyCd)
                .HasMaxLength(7)
                .IsUnicode(false)
                .HasColumnName("RLY_CD");
            entity.Property(e => e.Updatedby)
                .HasPrecision(6)
                .HasColumnName("UPDATEDBY");
            entity.Property(e => e.Updateddate)
                .HasColumnType("DATE")
                .HasColumnName("UPDATEDDATE");
            entity.Property(e => e.UserId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("USER_ID");
        });

        modelBuilder.Entity<T92State>(entity =>
        {
            entity.HasKey(e => e.StateCd).HasName("PK_STATE_CD");

            entity.ToTable("T92_STATE");

            entity.Property(e => e.StateCd)
                .HasPrecision(2)
                .HasColumnName("STATE_CD");
            entity.Property(e => e.SapStateCd)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("SAP_STATE_CD");
            entity.Property(e => e.StateName)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("STATE_NAME");
        });

        modelBuilder.Entity<T93IcType>(entity =>
        {
            entity.HasKey(e => e.IcTypeId).HasName("PK_IC_TYPE_ID");

            entity.ToTable("T93_IC_TYPES");

            entity.Property(e => e.IcTypeId)
                .HasPrecision(1)
                .HasColumnName("IC_TYPE_ID");
            entity.Property(e => e.IcType)
                .HasMaxLength(17)
                .IsUnicode(false)
                .HasColumnName("IC_TYPE");
        });

        modelBuilder.Entity<T94Bank>(entity =>
        {
            entity.HasKey(e => e.BankCd).HasName("PK_BANK_CD");

            entity.ToTable("T94_BANK");

            entity.Property(e => e.BankCd)
                .HasPrecision(6)
                .HasDefaultValueSql("\"IBSDEV\".\"T94_BANK_SEQ\".\"NEXTVAL\"")
                .HasColumnName("BANK_CD");
            entity.Property(e => e.BankName)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("BANK_NAME");
            entity.Property(e => e.Createdby)
                .HasPrecision(6)
                .HasColumnName("CREATEDBY");
            entity.Property(e => e.Createddate)
                .HasColumnType("DATE")
                .HasColumnName("CREATEDDATE");
            entity.Property(e => e.Datetime)
                .HasColumnType("DATE")
                .HasColumnName("DATETIME");
            entity.Property(e => e.FmisBankCd)
                .HasPrecision(4)
                .HasColumnName("FMIS_BANK_CD");
            entity.Property(e => e.Isdeleted)
                .HasPrecision(2)
                .HasColumnName("ISDELETED");
            entity.Property(e => e.Updatedby)
                .HasPrecision(6)
                .HasColumnName("UPDATEDBY");
            entity.Property(e => e.Updateddate)
                .HasColumnType("DATE")
                .HasColumnName("UPDATEDDATE");
            entity.Property(e => e.UserId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("USER_ID");
        });

        modelBuilder.Entity<T95AccountCode>(entity =>
        {
            entity.HasKey(e => e.AccCd).HasName("PK_ACC_CD");

            entity.ToTable("T95_ACCOUNT_CODES");

            entity.Property(e => e.AccCd)
                .HasPrecision(6)
                .ValueGeneratedNever()
                .HasColumnName("ACC_CD");
            entity.Property(e => e.AccDesc)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("ACC_DESC");
            entity.Property(e => e.Createdby)
                .HasPrecision(6)
                .HasColumnName("CREATEDBY");
            entity.Property(e => e.Createddate)
                .HasColumnType("DATE")
                .HasColumnName("CREATEDDATE");
            entity.Property(e => e.Isdeleted)
                .HasPrecision(2)
                .HasColumnName("ISDELETED");
            entity.Property(e => e.Updatedby)
                .HasPrecision(6)
                .HasColumnName("UPDATEDBY");
            entity.Property(e => e.Updateddate)
                .HasColumnType("DATE")
                .HasColumnName("UPDATEDDATE");
        });

        modelBuilder.Entity<T96Message>(entity =>
        {
            entity.HasKey(e => e.MessageId).HasName("PK_MESSAGE_ID");

            entity.ToTable("T96_MESSAGES");

            entity.Property(e => e.MessageId)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("\"IBSDEV\".\"T96_MESSAGES_SEQ\".\"NEXTVAL\"")
                .HasColumnType("NUMBER(38)")
                .HasColumnName("MESSAGE_ID");
            entity.Property(e => e.Createdby)
                .HasMaxLength(50)
                .IsUnicode(false)
                .ValueGeneratedOnAdd()
                .HasColumnName("CREATEDBY");
            entity.Property(e => e.Createddate)
                .ValueGeneratedOnAdd()
                .HasColumnType("TIMESTAMP(6) WITH TIME ZONE")
                .HasColumnName("CREATEDDATE");
            entity.Property(e => e.Datetime)
                .ValueGeneratedOnAdd()
                .HasColumnType("DATE")
                .HasColumnName("DATETIME");
            entity.Property(e => e.Isdeleted)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ISDELETED");
            entity.Property(e => e.Message)
                .HasMaxLength(500)
                .IsUnicode(false)
                .ValueGeneratedOnAdd()
                .HasColumnName("MESSAGE");
            entity.Property(e => e.Updatedby)
                .HasMaxLength(50)
                .IsUnicode(false)
                .ValueGeneratedOnAdd()
                .HasColumnName("UPDATEDBY");
            entity.Property(e => e.Updateddate)
                .ValueGeneratedOnAdd()
                .HasColumnType("TIMESTAMP(6) WITH TIME ZONE")
                .HasColumnName("UPDATEDDATE");
            entity.Property(e => e.UserId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .ValueGeneratedOnAdd()
                .IsFixedLength()
                .HasColumnName("USER_ID");
        });

        modelBuilder.Entity<T97ControlFile>(entity =>
        {
            entity.HasKey(e => e.Region).HasName("PK97_REION");

            entity.ToTable("T97_CONTROL_FILE");

            entity.Property(e => e.Region)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("REGION");
            entity.Property(e => e.AllowOldBillDt)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ALLOW_OLD_BILL_DT");
            entity.Property(e => e.Createdby)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CREATEDBY");
            entity.Property(e => e.Createddate)
                .HasColumnType("TIMESTAMP(6) WITH TIME ZONE")
                .HasColumnName("CREATEDDATE");
            entity.Property(e => e.GraceDays)
                .HasPrecision(6)
                .HasColumnName("GRACE_DAYS");
            entity.Property(e => e.Isdeleted)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ISDELETED");
            entity.Property(e => e.Updatedby)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("UPDATEDBY");
            entity.Property(e => e.Updateddate)
                .HasColumnType("TIMESTAMP(6) WITH TIME ZONE")
                .HasColumnName("UPDATEDDATE");
        });

        modelBuilder.Entity<T98Servicetax>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("T98_SERVICETAX");

            entity.Property(e => e.DtFrom)
                .HasColumnType("DATE")
                .HasColumnName("DT_FROM");
            entity.Property(e => e.DtTo)
                .HasColumnType("DATE")
                .HasColumnName("DT_TO");
            entity.Property(e => e.EduCess)
                .HasColumnType("NUMBER(5,2)")
                .HasColumnName("EDU_CESS");
            entity.Property(e => e.KrishiKalyanCess)
                .HasColumnType("NUMBER(5,2)")
                .HasColumnName("KRISHI_KALYAN_CESS");
            entity.Property(e => e.NetStaxRate)
                .HasColumnType("NUMBER(5,2)")
                .HasColumnName("NET_STAX_RATE");
            entity.Property(e => e.SheCess)
                .HasColumnType("NUMBER(5,2)")
                .HasColumnName("SHE_CESS");
            entity.Property(e => e.StaxRate)
                .HasColumnType("NUMBER(5,2)")
                .HasColumnName("STAX_RATE");
            entity.Property(e => e.SwachhBharatCess)
                .HasColumnType("NUMBER(5,2)")
                .HasColumnName("SWACHH_BHARAT_CESS");
        });

        modelBuilder.Entity<T99ClusterMaster>(entity =>
        {
            entity.HasKey(e => e.ClusterCode);

            entity.ToTable("T99_CLUSTER_MASTER");

            entity.Property(e => e.ClusterCode)
                .HasPrecision(3)
                .HasColumnName("CLUSTER_CODE");
            entity.Property(e => e.ClusterName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CLUSTER_NAME");
            entity.Property(e => e.Datetime)
                .HasColumnType("DATE")
                .HasColumnName("DATETIME");
            entity.Property(e => e.DepartmentName)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("DEPARTMENT_NAME");
            entity.Property(e => e.GeographicalPartition)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("GEOGRAPHICAL_PARTITION");
            entity.Property(e => e.HqArea)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("HQ_AREA");
            entity.Property(e => e.RegionCode)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("REGION_CODE");
            entity.Property(e => e.UserId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("USER_ID");
        });

        modelBuilder.Entity<Tblexception>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TBLEXCEPTION_PK");

            entity.ToTable("TBLEXCEPTION");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("\"IBSDEV\".\"TBLEXCEPTION_SEQ\".\"NEXTVAL\"")
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ID");
            entity.Property(e => e.Actionname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ACTIONNAME");
            entity.Property(e => e.Controllername)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CONTROLLERNAME");
            entity.Property(e => e.Createdby)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("CREATEDBY");
            entity.Property(e => e.Createddate)
                .HasColumnType("DATE")
                .HasColumnName("CREATEDDATE");
            entity.Property(e => e.Createip)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CREATEIP");
            entity.Property(e => e.Exception)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("EXCEPTION");
            entity.Property(e => e.Exceptionmessage)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("EXCEPTIONMESSAGE");
        });

        modelBuilder.Entity<Temp10Ic>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("TEMP10_IC");

            entity.Property(e => e.BkNo)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("BK_NO");
            entity.Property(e => e.BkStatus)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("BK_STATUS");
            entity.Property(e => e.CancelledSets)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("CANCELLED_SETS");
            entity.Property(e => e.IeCd)
                .HasPrecision(4)
                .HasColumnName("IE_CD");
            entity.Property(e => e.MissingSets)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("MISSING_SETS");
            entity.Property(e => e.Region)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("REGION");
            entity.Property(e => e.SetNo)
                .HasPrecision(3)
                .HasColumnName("SET_NO");
            entity.Property(e => e.SetStatus)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("SET_STATUS");
            entity.Property(e => e.StartSetNo)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("START_SET_NO");
        });

        modelBuilder.Entity<Temp22Bill>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("TEMP22_BILL");

            entity.Property(e => e.BillAmount)
                .HasColumnType("NUMBER(13,2)")
                .HasColumnName("BILL_AMOUNT");
            entity.Property(e => e.BillAmtCleared)
                .HasColumnType("NUMBER(13,2)")
                .HasColumnName("BILL_AMT_CLEARED");
            entity.Property(e => e.BillDt)
                .HasColumnType("DATE")
                .HasColumnName("BILL_DT");
            entity.Property(e => e.BillNo)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("BILL_NO");
            entity.Property(e => e.BpoCd)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("BPO_CD");
            entity.Property(e => e.BpoOrgn)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("BPO_ORGN");
            entity.Property(e => e.BpoRly)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("BPO_RLY");
            entity.Property(e => e.BpoType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("BPO_TYPE");
            entity.Property(e => e.ChqAmtPosted)
                .HasColumnType("NUMBER(13,2)")
                .HasColumnName("CHQ_AMT_POSTED");
            entity.Property(e => e.Outstanding)
                .HasColumnType("NUMBER(13,2)")
                .HasColumnName("OUTSTANDING");
            entity.Property(e => e.RegionCode)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("REGION_CODE");
            entity.Property(e => e.RetentionMoney)
                .HasColumnType("NUMBER(13,2)")
                .HasColumnName("RETENTION_MONEY");
            entity.Property(e => e.Tds)
                .HasColumnType("NUMBER(13,2)")
                .HasColumnName("TDS");
            entity.Property(e => e.WriteOffAmt)
                .HasColumnType("NUMBER(13,2)")
                .HasColumnName("WRITE_OFF_AMT");
        });

        modelBuilder.Entity<Temp2425Realisation>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("TEMP24_25_REALISATION");

            entity.Property(e => e.AdvanceAdjusted)
                .HasColumnType("NUMBER(12,2)")
                .HasColumnName("ADVANCE_ADJUSTED");
            entity.Property(e => e.AdvanceReceipt)
                .HasColumnType("NUMBER(12,2)")
                .HasColumnName("ADVANCE_RECEIPT");
            entity.Property(e => e.AmountAdjusted)
                .HasColumnType("NUMBER(12,2)")
                .HasColumnName("AMOUNT_ADJUSTED");
            entity.Property(e => e.AmtTransferred)
                .HasColumnType("NUMBER(12,2)")
                .HasColumnName("AMT_TRANSFERRED");
            entity.Property(e => e.BankCd)
                .HasPrecision(3)
                .HasColumnName("BANK_CD");
            entity.Property(e => e.BpoCd)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("BPO_CD");
            entity.Property(e => e.BpoOrgn)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("BPO_ORGN");
            entity.Property(e => e.BpoRly)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("BPO_RLY");
            entity.Property(e => e.BpoType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("BPO_TYPE");
            entity.Property(e => e.ChqDt)
                .HasColumnType("DATE")
                .HasColumnName("CHQ_DT");
            entity.Property(e => e.ChqNo)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("CHQ_NO");
            entity.Property(e => e.InspectionFee)
                .HasColumnType("NUMBER(12,2)")
                .HasColumnName("INSPECTION_FEE");
            entity.Property(e => e.LabFee)
                .HasColumnType("NUMBER(12,2)")
                .HasColumnName("LAB_FEE");
            entity.Property(e => e.MiscReceipt)
                .HasColumnType("NUMBER(12,2)")
                .HasColumnName("MISC_RECEIPT");
            entity.Property(e => e.MiscTransfer)
                .HasColumnType("NUMBER(12,2)")
                .HasColumnName("MISC_TRANSFER");
            entity.Property(e => e.NetRealisation)
                .HasColumnType("NUMBER(12,2)")
                .HasColumnName("NET_REALISATION");
            entity.Property(e => e.RealisationDt)
                .HasColumnType("DATE")
                .HasColumnName("REALISATION_DT");
            entity.Property(e => e.ReceiptFrCr)
                .HasColumnType("NUMBER(12,2)")
                .HasColumnName("RECEIPT_FR_CR");
            entity.Property(e => e.ReceiptFrEr)
                .HasColumnType("NUMBER(12,2)")
                .HasColumnName("RECEIPT_FR_ER");
            entity.Property(e => e.ReceiptFrNr)
                .HasColumnType("NUMBER(12,2)")
                .HasColumnName("RECEIPT_FR_NR");
            entity.Property(e => e.ReceiptFrSr)
                .HasColumnType("NUMBER(12,2)")
                .HasColumnName("RECEIPT_FR_SR");
            entity.Property(e => e.ReceiptFrWr)
                .HasColumnType("NUMBER(12,2)")
                .HasColumnName("RECEIPT_FR_WR");
            entity.Property(e => e.Region)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("REGION");
            entity.Property(e => e.Suspense)
                .HasColumnType("NUMBER(12,2)")
                .HasColumnName("SUSPENSE");
            entity.Property(e => e.TransferOldSystem)
                .HasColumnType("NUMBER(12,2)")
                .HasColumnName("TRANSFER_OLD_SYSTEM");
            entity.Property(e => e.TransferToCr)
                .HasColumnType("NUMBER(12,2)")
                .HasColumnName("TRANSFER_TO_CR");
            entity.Property(e => e.TransferToEr)
                .HasColumnType("NUMBER(12,2)")
                .HasColumnName("TRANSFER_TO_ER");
            entity.Property(e => e.TransferToNr)
                .HasColumnType("NUMBER(12,2)")
                .HasColumnName("TRANSFER_TO_NR");
            entity.Property(e => e.TransferToSr)
                .HasColumnType("NUMBER(12,2)")
                .HasColumnName("TRANSFER_TO_SR");
            entity.Property(e => e.TransferToWr)
                .HasColumnType("NUMBER(12,2)")
                .HasColumnName("TRANSFER_TO_WR");
        });

        modelBuilder.Entity<Temp25RvDetail>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("TEMP25_RV_DETAILS");

            entity.Property(e => e.AccCd)
                .HasPrecision(4)
                .HasColumnName("ACC_CD");
            entity.Property(e => e.Amount)
                .HasColumnType("NUMBER(12,2)")
                .HasColumnName("AMOUNT");
            entity.Property(e => e.AmountPosted)
                .HasColumnType("NUMBER(12,2)")
                .HasColumnName("AMOUNT_POSTED");
            entity.Property(e => e.AmtTransferred)
                .HasColumnType("NUMBER(12,2)")
                .HasColumnName("AMT_TRANSFERRED");
            entity.Property(e => e.BankCd)
                .HasPrecision(3)
                .HasColumnName("BANK_CD");
            entity.Property(e => e.BpoCd)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("BPO_CD");
            entity.Property(e => e.ChqDt)
                .HasColumnType("DATE")
                .HasColumnName("CHQ_DT");
            entity.Property(e => e.ChqNo)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("CHQ_NO");
            entity.Property(e => e.RegionCode)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("REGION_CODE");
            entity.Property(e => e.Sno)
                .HasPrecision(4)
                .HasColumnName("SNO");
            entity.Property(e => e.Suspense)
                .HasColumnType("NUMBER(12,2)")
                .HasColumnName("SUSPENSE");
            entity.Property(e => e.VchrDt)
                .HasColumnType("DATE")
                .HasColumnName("VCHR_DT");
            entity.Property(e => e.VchrNo)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("VCHR_NO");
        });

        modelBuilder.Entity<TempDeptWiseCall>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("TEMP_DEPT_WISE_CALLS");

            entity.Property(e => e.CaseNo)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CASE_NO");
            entity.Property(e => e.Department)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("DEPARTMENT");
            entity.Property(e => e.PoRecvDt)
                .HasColumnType("DATE")
                .HasColumnName("PO_RECV_DT");
            entity.Property(e => e.PoValue)
                .HasColumnType("NUMBER(16,2)")
                .HasColumnName("PO_VALUE");
        });

        modelBuilder.Entity<TempOnlineComplaint>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("TEMP_ONLINE_COMPLAINTS");

            entity.Property(e => e.BkNo)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("BK_NO");
            entity.Property(e => e.CaseNo)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CASE_NO");
            entity.Property(e => e.CoCd)
                .HasPrecision(3)
                .HasColumnName("CO_CD");
            entity.Property(e => e.ComplaintId)
                .HasMaxLength(12)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("COMPLAINT_ID");
            entity.Property(e => e.ConsigneeCd)
                .HasPrecision(8)
                .HasColumnName("CONSIGNEE_CD");
            entity.Property(e => e.ConsigneeDesig)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CONSIGNEE_DESIG");
            entity.Property(e => e.ConsigneeEmail)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CONSIGNEE_EMAIL");
            entity.Property(e => e.ConsigneeMobile)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("CONSIGNEE_MOBILE");
            entity.Property(e => e.ConsigneeName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("CONSIGNEE_NAME");
            entity.Property(e => e.IeCd)
                .HasPrecision(4)
                .HasColumnName("IE_CD");
            entity.Property(e => e.InspRegion)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("INSP_REGION");
            entity.Property(e => e.ItemDesc)
                .HasMaxLength(400)
                .IsUnicode(false)
                .HasColumnName("ITEM_DESC");
            entity.Property(e => e.ItemSrnoPo)
                .HasPrecision(3)
                .HasColumnName("ITEM_SRNO_PO");
            entity.Property(e => e.QtyOffered)
                .HasColumnType("NUMBER(11,3)")
                .HasColumnName("QTY_OFFERED");
            entity.Property(e => e.QtyRejected)
                .HasColumnType("NUMBER(11,3)")
                .HasColumnName("QTY_REJECTED");
            entity.Property(e => e.Rate)
                .HasColumnType("NUMBER(12,4)")
                .HasColumnName("RATE");
            entity.Property(e => e.RejMemoDt)
                .HasColumnType("DATE")
                .HasColumnName("REJ_MEMO_DT");
            entity.Property(e => e.RejMemoNo)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("REJ_MEMO_NO");
            entity.Property(e => e.RejectionReason)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("REJECTION_REASON");
            entity.Property(e => e.RejectionValue)
                .HasColumnType("NUMBER(15,2)")
                .HasColumnName("REJECTION_VALUE");
            entity.Property(e => e.Remarks)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("REMARKS");
            entity.Property(e => e.SetNo)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("SET_NO");
            entity.Property(e => e.Status)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("STATUS");
            entity.Property(e => e.TempCompRejReason)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("TEMP_COMP_REJ_REASON");
            entity.Property(e => e.TempComplaintDt)
                .HasColumnType("DATE")
                .HasColumnName("TEMP_COMPLAINT_DT");
            entity.Property(e => e.TempComplaintId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("TEMP_COMPLAINT_ID");
            entity.Property(e => e.UomCd)
                .HasPrecision(3)
                .HasColumnName("UOM_CD");
            entity.Property(e => e.VendCd)
                .HasPrecision(6)
                .HasColumnName("VEND_CD");
        });

        modelBuilder.Entity<TempVend>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("TEMP_VEND");

            entity.Property(e => e.City)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CITY");
            entity.Property(e => e.VendAdd1)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("VEND_ADD1");
            entity.Property(e => e.VendAdd2)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("VEND_ADD2");
            entity.Property(e => e.VendCd)
                .HasPrecision(6)
                .HasColumnName("VEND_CD");
            entity.Property(e => e.VendName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("VEND_NAME");
        });

        modelBuilder.Entity<TraineeEmployeeMaster>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("TRAINEE_EMPLOYEE_MASTER");

            entity.Property(e => e.Category)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("CATEGORY");
            entity.Property(e => e.CategoryOther)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("CATEGORY_OTHER");
            entity.Property(e => e.Descipline)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("DESCIPLINE");
            entity.Property(e => e.Dob)
                .HasColumnType("DATE")
                .HasColumnName("DOB");
            entity.Property(e => e.Doj)
                .HasColumnType("DATE")
                .HasColumnName("DOJ");
            entity.Property(e => e.EmpNo)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("EMP_NO");
            entity.Property(e => e.IeCd)
                .HasPrecision(4)
                .HasColumnName("IE_CD");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NAME");
            entity.Property(e => e.QualInstitute)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("QUAL_INSTITUTE");
            entity.Property(e => e.QualOther)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("QUAL_OTHER");
            entity.Property(e => e.Qualification)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("QUALIFICATION");
            entity.Property(e => e.Region)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("REGION");
        });

        modelBuilder.Entity<TrainingCourseMaster>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("TRAINING_COURSE_MASTER");

            entity.Property(e => e.Certificate)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("CERTIFICATE");
            entity.Property(e => e.CourseDurFr)
                .HasColumnType("DATE")
                .HasColumnName("COURSE_DUR_FR");
            entity.Property(e => e.CourseDurTo)
                .HasColumnType("DATE")
                .HasColumnName("COURSE_DUR_TO");
            entity.Property(e => e.CourseId)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("COURSE_ID");
            entity.Property(e => e.CourseInstitute)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("COURSE_INSTITUTE");
            entity.Property(e => e.CourseName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("COURSE_NAME");
            entity.Property(e => e.Fees)
                .HasColumnType("NUMBER(11,2)")
                .HasColumnName("FEES");
            entity.Property(e => e.Region)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("REGION");
            entity.Property(e => e.TrainingCategory)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("TRAINING_CATEGORY");
            entity.Property(e => e.TrainingField)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("TRAINING_FIELD");
            entity.Property(e => e.TrainingType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("TRAINING_TYPE");
            entity.Property(e => e.Validity)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("VALIDITY");
        });

        modelBuilder.Entity<TrainingDetail>(entity =>
        {
            entity.HasKey(e => new { e.IeCd, e.CourseId });

            entity.ToTable("TRAINING_DETAILS");

            entity.Property(e => e.IeCd)
                .HasPrecision(4)
                .HasColumnName("IE_CD");
            entity.Property(e => e.CourseId)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("COURSE_ID");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("USERS");

            entity.HasIndex(e => e.Email, "SYS_C008150").IsUnique();

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("NULL")
                .HasColumnType("DATE")
                .HasColumnName("CREATED_AT");
            entity.Property(e => e.Email)
                .HasMaxLength(191)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.EmailVerifiedAt)
                .HasDefaultValueSql("NULL")
                .HasColumnType("DATE")
                .HasColumnName("EMAIL_VERIFIED_AT");
            entity.Property(e => e.Id)
                .HasColumnType("NUMBER(20)")
                .HasColumnName("ID");
            entity.Property(e => e.Name)
                .HasMaxLength(191)
                .IsUnicode(false)
                .HasColumnName("NAME");
            entity.Property(e => e.Password)
                .HasMaxLength(191)
                .IsUnicode(false)
                .HasColumnName("PASSWORD");
            entity.Property(e => e.RememberToken)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasDefaultValueSql("NULL")
                .HasColumnName("REMEMBER_TOKEN");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("NULL")
                .HasColumnType("DATE")
                .HasColumnName("UPDATED_AT");
        });

        modelBuilder.Entity<V05Vendor>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("V05_VENDOR");

            entity.Property(e => e.VendCd)
                .HasPrecision(6)
                .HasColumnName("VEND_CD");
            entity.Property(e => e.Vendor)
                .HasMaxLength(305)
                .IsUnicode(false)
                .HasColumnName("VENDOR");
        });

        modelBuilder.Entity<V06Consignee>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("V06_CONSIGNEE");

            entity.Property(e => e.Consignee)
                .HasMaxLength(337)
                .IsUnicode(false)
                .HasColumnName("CONSIGNEE");
            entity.Property(e => e.ConsigneeCd)
                .HasPrecision(8)
                .HasColumnName("CONSIGNEE_CD");
        });

        modelBuilder.Entity<V12BillPayingOfficer>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("V12_BILL_PAYING_OFFICER");

            entity.Property(e => e.Bpo)
                .HasMaxLength(304)
                .IsUnicode(false)
                .HasColumnName("BPO");
            entity.Property(e => e.BpoCd)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("BPO_CD");
            entity.Property(e => e.SapCustCdBpo)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("SAP_CUST_CD_BPO");
        });

        modelBuilder.Entity<V17IeWiseDailyCallSummary>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("V17_IE_WISE_DAILY_CALL_SUMMARY");

            entity.Property(e => e.CallDate)
                .HasColumnType("DATE")
                .HasColumnName("CALL_DATE");
            entity.Property(e => e.CallDay)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("CALL_DAY");
            entity.Property(e => e.Calls)
                .HasColumnType("NUMBER")
                .HasColumnName("CALLS");
            entity.Property(e => e.IeName)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("IE_NAME");
            entity.Property(e => e.Region)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("REGION");
        });

        modelBuilder.Entity<V20Ic>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("V20_IC");

            entity.Property(e => e.BkNo)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("BK_NO");
            entity.Property(e => e.IeCd)
                .HasPrecision(4)
                .HasColumnName("IE_CD");
            entity.Property(e => e.SetNo)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("SET_NO");
        });

        modelBuilder.Entity<V22Bill>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("V22_BILL");

            entity.Property(e => e.AccGroup)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("ACC_GROUP");
            entity.Property(e => e.AckDt)
                .HasColumnType("DATE")
                .HasColumnName("ACK_DT");
            entity.Property(e => e.AckNo)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ACK_NO");
            entity.Property(e => e.AdvBill)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ADV_BILL");
            entity.Property(e => e.AmountReceived)
                .HasColumnType("NUMBER(13,2)")
                .HasColumnName("AMOUNT_RECEIVED");
            entity.Property(e => e.Au)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("AU");
            entity.Property(e => e.BillAmount)
                .HasColumnType("NUMBER(13,2)")
                .HasColumnName("BILL_AMOUNT");
            entity.Property(e => e.BillAmtCleared)
                .HasColumnType("NUMBER(13,2)")
                .HasColumnName("BILL_AMT_CLEARED");
            entity.Property(e => e.BillDt)
                .HasColumnType("DATE")
                .HasColumnName("BILL_DT");
            entity.Property(e => e.BillFinalised)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("BILL_FINALISED");
            entity.Property(e => e.BillNo)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("BILL_NO");
            entity.Property(e => e.BillResentCount)
                .HasPrecision(1)
                .HasColumnName("BILL_RESENT_COUNT");
            entity.Property(e => e.BillResentStatus)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("BILL_RESENT_STATUS");
            entity.Property(e => e.BillStatus)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("BILL_STATUS");
            entity.Property(e => e.BkNo)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("BK_NO");
            entity.Property(e => e.BpoAdd)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("BPO_ADD");
            entity.Property(e => e.BpoCd)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("BPO_CD");
            entity.Property(e => e.BpoCity)
                .HasMaxLength(101)
                .IsUnicode(false)
                .HasColumnName("BPO_CITY");
            entity.Property(e => e.BpoName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("BPO_NAME");
            entity.Property(e => e.BpoOrgn)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("BPO_ORGN");
            entity.Property(e => e.BpoRly)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("BPO_RLY");
            entity.Property(e => e.BpoType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("BPO_TYPE");
            entity.Property(e => e.CallInstalmentNo)
                .HasMaxLength(46)
                .IsUnicode(false)
                .HasColumnName("CALL_INSTALMENT_NO");
            entity.Property(e => e.CaseNo)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CASE_NO");
            entity.Property(e => e.Cgst)
                .HasColumnType("NUMBER(10,2)")
                .HasColumnName("CGST");
            entity.Property(e => e.CnoteAmount)
                .HasColumnType("NUMBER(13,2)")
                .HasColumnName("CNOTE_AMOUNT");
            entity.Property(e => e.Consignee)
                .HasMaxLength(132)
                .IsUnicode(false)
                .HasColumnName("CONSIGNEE");
            entity.Property(e => e.ConsigneeAdd1)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("CONSIGNEE_ADD1");
            entity.Property(e => e.ConsigneeAdd2)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("CONSIGNEE_ADD2");
            entity.Property(e => e.ConsigneeCd)
                .HasPrecision(8)
                .HasColumnName("CONSIGNEE_CD");
            entity.Property(e => e.ConsigneeCity)
                .HasMaxLength(101)
                .IsUnicode(false)
                .HasColumnName("CONSIGNEE_CITY");
            entity.Property(e => e.CreditDocId)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("CREDIT_DOC_ID");
            entity.Property(e => e.DigBillGenDt)
                .HasColumnType("DATE")
                .HasColumnName("DIG_BILL_GEN_DT");
            entity.Property(e => e.EduCess)
                .HasColumnType("NUMBER(10,2)")
                .HasColumnName("EDU_CESS");
            entity.Property(e => e.FeeRate)
                .HasColumnType("NUMBER(11,4)")
                .HasColumnName("FEE_RATE");
            entity.Property(e => e.FeeType)
                .HasMaxLength(7)
                .IsUnicode(false)
                .HasColumnName("FEE_TYPE");
            entity.Property(e => e.IcDt)
                .HasColumnType("DATE")
                .HasColumnName("IC_DT");
            entity.Property(e => e.IcNo)
                .HasMaxLength(29)
                .IsUnicode(false)
                .HasColumnName("IC_NO");
            entity.Property(e => e.IeCd)
                .HasPrecision(4)
                .HasColumnName("IE_CD");
            entity.Property(e => e.IeCoCd)
                .HasPrecision(3)
                .HasColumnName("IE_CO_CD");
            entity.Property(e => e.Igst)
                .HasColumnType("NUMBER(10,2)")
                .HasColumnName("IGST");
            entity.Property(e => e.InspFee)
                .HasColumnType("NUMBER(13,2)")
                .HasColumnName("INSP_FEE");
            entity.Property(e => e.InvoiceNo)
                .HasMaxLength(14)
                .IsUnicode(false)
                .HasColumnName("INVOICE_NO");
            entity.Property(e => e.InvoiceSuppDocs)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("INVOICE_SUPP_DOCS");
            entity.Property(e => e.IrfcBpoAdd)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("IRFC_BPO_ADD");
            entity.Property(e => e.IrfcBpoCd)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("IRFC_BPO_CD");
            entity.Property(e => e.IrfcBpoCity)
                .HasMaxLength(101)
                .IsUnicode(false)
                .HasColumnName("IRFC_BPO_CITY");
            entity.Property(e => e.IrfcBpoName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("IRFC_BPO_NAME");
            entity.Property(e => e.IrfcBpoRly)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("IRFC_BPO_RLY");
            entity.Property(e => e.IrfcFunded)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("IRFC_FUNDED");
            entity.Property(e => e.IrnNo)
                .HasMaxLength(64)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("IRN_NO");
            entity.Property(e => e.KrishiKalyanCess)
                .HasColumnType("NUMBER(10,2)")
                .HasColumnName("KRISHI_KALYAN_CESS");
            entity.Property(e => e.LoRemarks)
                .HasMaxLength(400)
                .IsUnicode(false)
                .HasColumnName("LO_REMARKS");
            entity.Property(e => e.MaterialValue)
                .HasColumnType("NUMBER(15,2)")
                .HasColumnName("MATERIAL_VALUE");
            entity.Property(e => e.MaxFee)
                .HasPrecision(11)
                .HasColumnName("MAX_FEE");
            entity.Property(e => e.MinFee)
                .HasPrecision(11)
                .HasColumnName("MIN_FEE");
            entity.Property(e => e.PoDt)
                .HasColumnType("DATE")
                .HasColumnName("PO_DT");
            entity.Property(e => e.PoNo)
                .HasMaxLength(75)
                .IsUnicode(false)
                .HasColumnName("PO_NO");
            entity.Property(e => e.PoOrLetter)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("PO_OR_LETTER");
            entity.Property(e => e.PoSource)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("PO_SOURCE");
            entity.Property(e => e.QrCode)
                .HasColumnType("NCLOB")
                .HasColumnName("QR_CODE");
            entity.Property(e => e.RecipientGstinNo)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("RECIPIENT_GSTIN_NO");
            entity.Property(e => e.RegionCode)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("REGION_CODE");
            entity.Property(e => e.Remarks)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("REMARKS");
            entity.Property(e => e.RetentionMoney)
                .HasColumnType("NUMBER(13,2)")
                .HasColumnName("RETENTION_MONEY");
            entity.Property(e => e.SapCustCdBpo)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("SAP_CUST_CD_BPO");
            entity.Property(e => e.SapCustCdCon)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("SAP_CUST_CD_CON");
            entity.Property(e => e.SapCustCdIrfc)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("SAP_CUST_CD_IRFC");
            entity.Property(e => e.SentToSap)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("SENT_TO_SAP");
            entity.Property(e => e.ServTaxRate)
                .HasColumnType("NUMBER(5,2)")
                .HasColumnName("SERV_TAX_RATE");
            entity.Property(e => e.ServiceTax)
                .HasColumnType("NUMBER(11,2)")
                .HasColumnName("SERVICE_TAX");
            entity.Property(e => e.SetNo)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("SET_NO");
            entity.Property(e => e.Sgst)
                .HasColumnType("NUMBER(10,2)")
                .HasColumnName("SGST");
            entity.Property(e => e.SheCess)
                .HasColumnType("NUMBER(10,2)")
                .HasColumnName("SHE_CESS");
            entity.Property(e => e.SwachhBharatCess)
                .HasColumnType("NUMBER(10,2)")
                .HasColumnName("SWACHH_BHARAT_CESS");
            entity.Property(e => e.TaxType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("TAX_TYPE");
            entity.Property(e => e.Tds)
                .HasColumnType("NUMBER(13,2)")
                .HasColumnName("TDS");
            entity.Property(e => e.TdsCgst)
                .HasColumnType("NUMBER(10,2)")
                .HasColumnName("TDS_CGST");
            entity.Property(e => e.TdsDt)
                .HasColumnType("DATE")
                .HasColumnName("TDS_DT");
            entity.Property(e => e.TdsIgst)
                .HasColumnType("NUMBER(10,2)")
                .HasColumnName("TDS_IGST");
            entity.Property(e => e.TdsSgst)
                .HasColumnType("NUMBER(10,2)")
                .HasColumnName("TDS_SGST");
            entity.Property(e => e.UserId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("USER_ID");
            entity.Property(e => e.VendAdd1)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("VEND_ADD1");
            entity.Property(e => e.VendAdd2)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("VEND_ADD2");
            entity.Property(e => e.VendCd)
                .HasPrecision(6)
                .HasColumnName("VEND_CD");
            entity.Property(e => e.VendName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("VEND_NAME");
            entity.Property(e => e.VendorCity)
                .HasMaxLength(101)
                .IsUnicode(false)
                .HasColumnName("VENDOR_CITY");
            entity.Property(e => e.Visits)
                .HasPrecision(3)
                .HasColumnName("VISITS");
            entity.Property(e => e.WriteOffAmt)
                .HasColumnType("NUMBER(13,2)")
                .HasColumnName("WRITE_OFF_AMT");
            entity.Property(e => e.YrMth)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("YR_MTH");
        });

        modelBuilder.Entity<V22aBillingSummary>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("V22A_BILLING_SUMMARY");

            entity.Property(e => e.BillAmount)
                .HasColumnType("NUMBER")
                .HasColumnName("BILL_AMOUNT");
            entity.Property(e => e.BillingYrMth)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("BILLING_YR_MTH");
            entity.Property(e => e.Cgst)
                .HasColumnType("NUMBER")
                .HasColumnName("CGST");
            entity.Property(e => e.EduCess)
                .HasColumnType("NUMBER")
                .HasColumnName("EDU_CESS");
            entity.Property(e => e.Igst)
                .HasColumnType("NUMBER")
                .HasColumnName("IGST");
            entity.Property(e => e.InspFee)
                .HasColumnType("NUMBER")
                .HasColumnName("INSP_FEE");
            entity.Property(e => e.KrishiKalyanCess)
                .HasColumnType("NUMBER")
                .HasColumnName("KRISHI_KALYAN_CESS");
            entity.Property(e => e.NoOfBillls)
                .HasColumnType("NUMBER")
                .HasColumnName("NO_OF_BILLLS");
            entity.Property(e => e.RegionCode)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("REGION_CODE");
            entity.Property(e => e.Sector)
                .HasMaxLength(17)
                .IsUnicode(false)
                .HasColumnName("SECTOR");
            entity.Property(e => e.ServiceTax)
                .HasColumnType("NUMBER")
                .HasColumnName("SERVICE_TAX");
            entity.Property(e => e.Sgst)
                .HasColumnType("NUMBER")
                .HasColumnName("SGST");
            entity.Property(e => e.SheCess)
                .HasColumnType("NUMBER")
                .HasColumnName("SHE_CESS");
            entity.Property(e => e.SwachhBharatCess)
                .HasColumnType("NUMBER")
                .HasColumnName("SWACHH_BHARAT_CESS");
        });

        modelBuilder.Entity<V22bOutstandingBill>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("V22B_OUTSTANDING_BILLS");

            entity.Property(e => e.AmountOutstanding)
                .HasColumnType("NUMBER")
                .HasColumnName("AMOUNT_OUTSTANDING");
            entity.Property(e => e.AmountPosted)
                .HasColumnType("NUMBER")
                .HasColumnName("AMOUNT_POSTED");
            entity.Property(e => e.AmountRealised)
                .HasColumnType("NUMBER")
                .HasColumnName("AMOUNT_REALISED");
            entity.Property(e => e.Au)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("AU");
            entity.Property(e => e.BillAmount)
                .HasColumnType("NUMBER")
                .HasColumnName("BILL_AMOUNT");
            entity.Property(e => e.BillDt)
                .HasColumnType("DATE")
                .HasColumnName("BILL_DT");
            entity.Property(e => e.BillNo)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("BILL_NO");
            entity.Property(e => e.BkNo)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("BK_NO");
            entity.Property(e => e.BpoAdd)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("BPO_ADD");
            entity.Property(e => e.BpoCd)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("BPO_CD");
            entity.Property(e => e.BpoCity)
                .HasMaxLength(101)
                .IsUnicode(false)
                .HasColumnName("BPO_CITY");
            entity.Property(e => e.BpoName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("BPO_NAME");
            entity.Property(e => e.BpoOrgn)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("BPO_ORGN");
            entity.Property(e => e.BpoRegion)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("BPO_REGION");
            entity.Property(e => e.BpoRly)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("BPO_RLY");
            entity.Property(e => e.BpoType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("BPO_TYPE");
            entity.Property(e => e.CaseNo)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CASE_NO");
            entity.Property(e => e.CnoteAmount)
                .HasColumnType("NUMBER")
                .HasColumnName("CNOTE_AMOUNT");
            entity.Property(e => e.Consignee)
                .HasMaxLength(132)
                .IsUnicode(false)
                .HasColumnName("CONSIGNEE");
            entity.Property(e => e.ConsigneeAdd1)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("CONSIGNEE_ADD1");
            entity.Property(e => e.ConsigneeAdd2)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("CONSIGNEE_ADD2");
            entity.Property(e => e.ConsigneeCd)
                .HasPrecision(8)
                .HasColumnName("CONSIGNEE_CD");
            entity.Property(e => e.ConsigneeCity)
                .HasMaxLength(101)
                .IsUnicode(false)
                .HasColumnName("CONSIGNEE_CITY");
            entity.Property(e => e.FinYr)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("FIN_YR");
            entity.Property(e => e.IeCd)
                .HasPrecision(4)
                .HasColumnName("IE_CD");
            entity.Property(e => e.LoRemarks)
                .HasMaxLength(400)
                .IsUnicode(false)
                .HasColumnName("LO_REMARKS");
            entity.Property(e => e.PoDt)
                .HasColumnType("DATE")
                .HasColumnName("PO_DT");
            entity.Property(e => e.PoNo)
                .HasMaxLength(75)
                .IsUnicode(false)
                .HasColumnName("PO_NO");
            entity.Property(e => e.PoOrLetter)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("PO_OR_LETTER");
            entity.Property(e => e.RegionCode)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("REGION_CODE");
            entity.Property(e => e.RetentionMoney)
                .HasColumnType("NUMBER")
                .HasColumnName("RETENTION_MONEY");
            entity.Property(e => e.SapCustCdBpo)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("SAP_CUST_CD_BPO");
            entity.Property(e => e.SetNo)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("SET_NO");
            entity.Property(e => e.Tds)
                .HasColumnType("NUMBER")
                .HasColumnName("TDS");
            entity.Property(e => e.TdsCgst)
                .HasColumnType("NUMBER")
                .HasColumnName("TDS_CGST");
            entity.Property(e => e.TdsIgst)
                .HasColumnType("NUMBER")
                .HasColumnName("TDS_IGST");
            entity.Property(e => e.TdsSgst)
                .HasColumnType("NUMBER")
                .HasColumnName("TDS_SGST");
            entity.Property(e => e.VendCd)
                .HasPrecision(6)
                .HasColumnName("VEND_CD");
            entity.Property(e => e.VendName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("VEND_NAME");
            entity.Property(e => e.VendorCity)
                .HasMaxLength(101)
                .IsUnicode(false)
                .HasColumnName("VENDOR_CITY");
            entity.Property(e => e.WriteOffAmt)
                .HasColumnType("NUMBER")
                .HasColumnName("WRITE_OFF_AMT");
            entity.Property(e => e.YrMth)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("YR_MTH");
        });

        modelBuilder.Entity<V23BillItem>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("V23_BILL_ITEMS");

            entity.Property(e => e.BasicValue)
                .HasColumnType("NUMBER(13,2)")
                .HasColumnName("BASIC_VALUE");
            entity.Property(e => e.BillNo)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("BILL_NO");
            entity.Property(e => e.Discount)
                .HasColumnType("NUMBER(10,2)")
                .HasColumnName("DISCOUNT");
            entity.Property(e => e.DiscountPer)
                .HasColumnType("NUMBER(10,2)")
                .HasColumnName("DISCOUNT_PER");
            entity.Property(e => e.DiscountType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("DISCOUNT_TYPE");
            entity.Property(e => e.Excise)
                .HasColumnType("NUMBER(11,2)")
                .HasColumnName("EXCISE");
            entity.Property(e => e.ExcisePer)
                .HasColumnType("NUMBER(10,2)")
                .HasColumnName("EXCISE_PER");
            entity.Property(e => e.ExciseType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("EXCISE_TYPE");
            entity.Property(e => e.ItemDesc)
                .HasMaxLength(400)
                .IsUnicode(false)
                .HasColumnName("ITEM_DESC");
            entity.Property(e => e.ItemSrno)
                .HasPrecision(4)
                .HasColumnName("ITEM_SRNO");
            entity.Property(e => e.OtChargePer)
                .HasColumnType("NUMBER(10,2)")
                .HasColumnName("OT_CHARGE_PER");
            entity.Property(e => e.OtChargeType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("OT_CHARGE_TYPE");
            entity.Property(e => e.OtherCharges)
                .HasColumnType("NUMBER(10,2)")
                .HasColumnName("OTHER_CHARGES");
            entity.Property(e => e.Qty)
                .HasColumnType("NUMBER(12,4)")
                .HasColumnName("QTY");
            entity.Property(e => e.Rate)
                .HasColumnType("NUMBER(11,2)")
                .HasColumnName("RATE");
            entity.Property(e => e.SalesTax)
                .HasColumnType("NUMBER(13,2)")
                .HasColumnName("SALES_TAX");
            entity.Property(e => e.SalesTaxPer)
                .HasColumnType("NUMBER(10,2)")
                .HasColumnName("SALES_TAX_PER");
            entity.Property(e => e.UomFactor)
                .HasMaxLength(55)
                .IsUnicode(false)
                .HasColumnName("UOM_FACTOR");
            entity.Property(e => e.UomSDesc)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("UOM_S_DESC");
            entity.Property(e => e.Value)
                .HasColumnType("NUMBER(15,2)")
                .HasColumnName("VALUE");
        });

        modelBuilder.Entity<V2425Realisation>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("V24_25_REALISATION");

            entity.Property(e => e.AccCd)
                .HasPrecision(4)
                .HasColumnName("ACC_CD");
            entity.Property(e => e.AmountAdjusted)
                .HasColumnType("NUMBER")
                .HasColumnName("AMOUNT_ADJUSTED");
            entity.Property(e => e.AmtTransferred)
                .HasColumnType("NUMBER")
                .HasColumnName("AMT_TRANSFERRED");
            entity.Property(e => e.BankCd)
                .HasPrecision(3)
                .HasColumnName("BANK_CD");
            entity.Property(e => e.BpoCd)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("BPO_CD");
            entity.Property(e => e.BpoOrgn)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("BPO_ORGN");
            entity.Property(e => e.BpoRly)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("BPO_RLY");
            entity.Property(e => e.BpoType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("BPO_TYPE");
            entity.Property(e => e.ChqDt)
                .HasColumnType("DATE")
                .HasColumnName("CHQ_DT");
            entity.Property(e => e.ChqNo)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("CHQ_NO");
            entity.Property(e => e.RealisationDt)
                .HasColumnType("DATE")
                .HasColumnName("REALISATION_DT");
            entity.Property(e => e.RealisedAmt)
                .HasColumnType("NUMBER")
                .HasColumnName("REALISED_AMT");
            entity.Property(e => e.SuspenseAmt)
                .HasColumnType("NUMBER")
                .HasColumnName("SUSPENSE_AMT");
            entity.Property(e => e.VchrNo)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("VCHR_NO");
            entity.Property(e => e.VchrType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("VCHR_TYPE");
        });

        modelBuilder.Entity<V252709>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("V25_2709");

            entity.Property(e => e.Amount)
                .HasColumnType("NUMBER(12,2)")
                .HasColumnName("AMOUNT");
            entity.Property(e => e.AmountAdjusted)
                .HasColumnType("NUMBER(12,2)")
                .HasColumnName("AMOUNT_ADJUSTED");
            entity.Property(e => e.Balance)
                .HasColumnType("NUMBER(12,2)")
                .HasColumnName("BALANCE");
            entity.Property(e => e.BankCd)
                .HasPrecision(3)
                .HasColumnName("BANK_CD");
            entity.Property(e => e.CaseNo)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CASE_NO");
            entity.Property(e => e.ChqDt)
                .HasColumnType("DATE")
                .HasColumnName("CHQ_DT");
            entity.Property(e => e.ChqNo)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("CHQ_NO");
            entity.Property(e => e.Region)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("REGION");
            entity.Property(e => e.VchrDt)
                .HasColumnType("DATE")
                .HasColumnName("VCHR_DT");
            entity.Property(e => e.VendCd)
                .HasPrecision(6)
                .HasColumnName("VEND_CD");
            entity.Property(e => e.VendName)
                .HasMaxLength(151)
                .IsUnicode(false)
                .HasColumnName("VEND_NAME");
        });

        modelBuilder.Entity<V25InspectionDetail>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("V25_INSPECTION_DETAILS");

            entity.Property(e => e.BillAmount)
                .HasColumnType("NUMBER(13,2)")
                .HasColumnName("BILL_AMOUNT");
            entity.Property(e => e.BkNo)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("BK_NO");
            entity.Property(e => e.BpoCd)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("BPO_CD");
            entity.Property(e => e.Consignee)
                .HasMaxLength(183)
                .IsUnicode(false)
                .HasColumnName("CONSIGNEE");
            entity.Property(e => e.ConsigneeCd)
                .HasPrecision(8)
                .HasColumnName("CONSIGNEE_CD");
            entity.Property(e => e.IcDt)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("IC_DT");
            entity.Property(e => e.IcNo)
                .HasMaxLength(29)
                .IsUnicode(false)
                .HasColumnName("IC_NO");
            entity.Property(e => e.IeCd)
                .HasPrecision(4)
                .HasColumnName("IE_CD");
            entity.Property(e => e.IeName)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("IE_NAME");
            entity.Property(e => e.ItemDescPo)
                .HasMaxLength(400)
                .IsUnicode(false)
                .HasColumnName("ITEM_DESC_PO");
            entity.Property(e => e.ItemSrnoPo)
                .HasPrecision(4)
                .HasColumnName("ITEM_SRNO_PO");
            entity.Property(e => e.MaterialValue)
                .HasColumnType("NUMBER(15,2)")
                .HasColumnName("MATERIAL_VALUE");
            entity.Property(e => e.MfgCd)
                .HasPrecision(6)
                .HasColumnName("MFG_CD");
            entity.Property(e => e.PoDt)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("PO_DT");
            entity.Property(e => e.PoNo)
                .HasMaxLength(75)
                .IsUnicode(false)
                .HasColumnName("PO_NO");
            entity.Property(e => e.PurchaserCd)
                .HasPrecision(8)
                .HasColumnName("PURCHASER_CD");
            entity.Property(e => e.SetNo)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("SET_NO");
        });

        modelBuilder.Entity<V40ConsigneeComplaint>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("V40_CONSIGNEE_COMPLAINTS");

            entity.Property(e => e.Action)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("ACTION");
            entity.Property(e => e.ActionProposed)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ACTION_PROPOSED");
            entity.Property(e => e.ActionProposedDt)
                .HasColumnType("DATE")
                .HasColumnName("ACTION_PROPOSED_DT");
            entity.Property(e => e.AnyOther)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("ANY_OTHER");
            entity.Property(e => e.BkNo)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("BK_NO");
            entity.Property(e => e.BpoType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("BPO_TYPE");
            entity.Property(e => e.CapaStatus)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("CAPA_STATUS");
            entity.Property(e => e.CaseNo)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CASE_NO");
            entity.Property(e => e.ChksheetStatus)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CHKSHEET_STATUS");
            entity.Property(e => e.CompRecvRegion)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("COMP_RECV_REGION");
            entity.Property(e => e.CompRecvRegnName)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("COMP_RECV_REGN_NAME");
            entity.Property(e => e.ComplaintDt)
                .HasColumnType("DATE")
                .HasColumnName("COMPLAINT_DT");
            entity.Property(e => e.ComplaintId)
                .HasMaxLength(12)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("COMPLAINT_ID");
            entity.Property(e => e.ConclusionDt)
                .HasColumnType("DATE")
                .HasColumnName("CONCLUSION_DT");
            entity.Property(e => e.ConsigneeAddr)
                .HasMaxLength(204)
                .IsUnicode(false)
                .HasColumnName("CONSIGNEE_ADDR");
            entity.Property(e => e.ConsigneeCd)
                .HasPrecision(8)
                .HasColumnName("CONSIGNEE_CD");
            entity.Property(e => e.ConsigneeCity)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CONSIGNEE_CITY");
            entity.Property(e => e.ConsigneeName)
                .HasMaxLength(528)
                .IsUnicode(false)
                .HasColumnName("CONSIGNEE_NAME");
            entity.Property(e => e.DandarStatus)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("DANDAR_STATUS");
            entity.Property(e => e.Datetime)
                .HasColumnType("DATE")
                .HasColumnName("DATETIME");
            entity.Property(e => e.DefectCd)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("DEFECT_CD");
            entity.Property(e => e.IcDt)
                .HasColumnType("DATE")
                .HasColumnName("IC_DT");
            entity.Property(e => e.IcNo)
                .HasMaxLength(29)
                .IsUnicode(false)
                .HasColumnName("IC_NO");
            entity.Property(e => e.IeCd)
                .HasPrecision(4)
                .HasColumnName("IE_CD");
            entity.Property(e => e.IeCoCd)
                .HasPrecision(3)
                .HasColumnName("IE_CO_CD");
            entity.Property(e => e.IeCoName)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("IE_CO_NAME");
            entity.Property(e => e.IeJiRemarks)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("IE_JI_REMARKS");
            entity.Property(e => e.IeJiRemarksDt)
                .HasColumnType("DATE")
                .HasColumnName("IE_JI_REMARKS_DT");
            entity.Property(e => e.IeName)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("IE_NAME");
            entity.Property(e => e.InspRegion)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("INSP_REGION");
            entity.Property(e => e.InspRegionName)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("INSP_REGION_NAME");
            entity.Property(e => e.ItemDesc)
                .HasMaxLength(400)
                .IsUnicode(false)
                .HasColumnName("ITEM_DESC");
            entity.Property(e => e.ItemSrnoPo)
                .HasPrecision(4)
                .HasColumnName("ITEM_SRNO_PO");
            entity.Property(e => e.JiApprovalDt)
                .HasColumnType("DATE")
                .HasColumnName("JI_APPROVAL_DT");
            entity.Property(e => e.JiApprovedBy)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("JI_APPROVED_BY");
            entity.Property(e => e.JiDt)
                .HasColumnType("DATE")
                .HasColumnName("JI_DT");
            entity.Property(e => e.JiFixDt)
                .HasColumnType("DATE")
                .HasColumnName("JI_FIX_DT");
            entity.Property(e => e.JiIeCd)
                .HasPrecision(4)
                .HasColumnName("JI_IE_CD");
            entity.Property(e => e.JiIeRemarks)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("JI_IE_REMARKS");
            entity.Property(e => e.JiIeRemarksDt)
                .HasColumnType("DATE")
                .HasColumnName("JI_IE_REMARKS_DT");
            entity.Property(e => e.JiRegion)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("JI_REGION");
            entity.Property(e => e.JiRequired)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("JI_REQUIRED");
            entity.Property(e => e.JiSno)
                .HasMaxLength(13)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("JI_SNO");
            entity.Property(e => e.JiStatusCd)
                .HasPrecision(2)
                .HasColumnName("JI_STATUS_CD");
            entity.Property(e => e.L5noPo)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("L5NO_PO");
            entity.Property(e => e.NoJiReason)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("NO_JI_REASON");
            entity.Property(e => e.PenaltyDt)
                .HasColumnType("DATE")
                .HasColumnName("PENALTY_DT");
            entity.Property(e => e.PenaltyType)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("PENALTY_TYPE");
            entity.Property(e => e.PoDt)
                .HasColumnType("DATE")
                .HasColumnName("PO_DT");
            entity.Property(e => e.PoNo)
                .HasMaxLength(75)
                .IsUnicode(false)
                .HasColumnName("PO_NO");
            entity.Property(e => e.QtyOffered)
                .HasColumnType("NUMBER(12,4)")
                .HasColumnName("QTY_OFFERED");
            entity.Property(e => e.QtyRejected)
                .HasColumnType("NUMBER(12,4)")
                .HasColumnName("QTY_REJECTED");
            entity.Property(e => e.Rate)
                .HasColumnType("NUMBER(13,4)")
                .HasColumnName("RATE");
            entity.Property(e => e.RejMemoDt)
                .HasColumnType("DATE")
                .HasColumnName("REJ_MEMO_DT");
            entity.Property(e => e.RejMemoNo)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("REJ_MEMO_NO");
            entity.Property(e => e.RejectionReason)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("REJECTION_REASON");
            entity.Property(e => e.RejectionValue)
                .HasColumnType("NUMBER(15,2)")
                .HasColumnName("REJECTION_VALUE");
            entity.Property(e => e.Remarks)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("REMARKS");
            entity.Property(e => e.RlyCd)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("RLY_CD");
            entity.Property(e => e.RootCauseAnalysis)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("ROOT_CAUSE_ANALYSIS");
            entity.Property(e => e.SetNo)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("SET_NO");
            entity.Property(e => e.Status)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("STATUS");
            entity.Property(e => e.TechRef)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("TECH_REF");
            entity.Property(e => e.UomCd)
                .HasPrecision(3)
                .HasColumnName("UOM_CD");
            entity.Property(e => e.UomSDesc)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("UOM_S_DESC");
            entity.Property(e => e.UserId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("USER_ID");
            entity.Property(e => e.VendCd)
                .HasPrecision(6)
                .HasColumnName("VEND_CD");
            entity.Property(e => e.Vendor)
                .HasMaxLength(1220)
                .IsUnicode(false)
                .HasColumnName("VENDOR");
        });

        modelBuilder.Entity<V55LabInvoice>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("V55_LAB_INVOICE");

            entity.Property(e => e.AckDt)
                .HasColumnType("DATE")
                .HasColumnName("ACK_DT");
            entity.Property(e => e.AckNo)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ACK_NO");
            entity.Property(e => e.BillAmount)
                .HasColumnType("NUMBER(13,2)")
                .HasColumnName("BILL_AMOUNT");
            entity.Property(e => e.BillFinalised)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("BILL_FINALISED");
            entity.Property(e => e.BpoAdd)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("BPO_ADD");
            entity.Property(e => e.BpoCd)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("BPO_CD");
            entity.Property(e => e.BpoCity)
                .HasMaxLength(101)
                .IsUnicode(false)
                .HasColumnName("BPO_CITY");
            entity.Property(e => e.BpoName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("BPO_NAME");
            entity.Property(e => e.BpoOrgn)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("BPO_ORGN");
            entity.Property(e => e.BpoRly)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("BPO_RLY");
            entity.Property(e => e.BpoType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("BPO_TYPE");
            entity.Property(e => e.CaseNo)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CASE_NO");
            entity.Property(e => e.CustomerRefNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CUSTOMER_REF_NO");
            entity.Property(e => e.IeCd)
                .HasPrecision(4)
                .HasColumnName("IE_CD");
            entity.Property(e => e.IeName)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("IE_NAME");
            entity.Property(e => e.InvSCity)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("INV_S_CITY");
            entity.Property(e => e.InvoiceDt)
                .HasColumnType("DATE")
                .HasColumnName("INVOICE_DT");
            entity.Property(e => e.InvoiceNo)
                .HasMaxLength(14)
                .IsUnicode(false)
                .HasColumnName("INVOICE_NO");
            entity.Property(e => e.IrnNo)
                .HasMaxLength(64)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("IRN_NO");
            entity.Property(e => e.QrCode)
                .HasColumnType("NCLOB")
                .HasColumnName("QR_CODE");
            entity.Property(e => e.RecipientGstinNo)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("RECIPIENT_GSTIN_NO");
            entity.Property(e => e.RegionCode)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("REGION_CODE");
            entity.Property(e => e.SampleRegNo)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("SAMPLE_REG_NO");
            entity.Property(e => e.SapCustCdBpo)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("SAP_CUST_CD_BPO");
            entity.Property(e => e.SentToSap)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("SENT_TO_SAP");
            entity.Property(e => e.TotalBillAmount)
                .HasColumnType("NUMBER")
                .HasColumnName("TOTAL_BILL_AMOUNT");
            entity.Property(e => e.TotalCgst)
                .HasColumnType("NUMBER(10,2)")
                .HasColumnName("TOTAL_CGST");
            entity.Property(e => e.TotalIgst)
                .HasColumnType("NUMBER(10,2)")
                .HasColumnName("TOTAL_IGST");
            entity.Property(e => e.TotalSgst)
                .HasColumnType("NUMBER(10,2)")
                .HasColumnName("TOTAL_SGST");
            entity.Property(e => e.TransactionNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("TRANSACTION_NO");
        });

        modelBuilder.Entity<V56LabInvoiceDetail>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("V56_LAB_INVOICE_DETAILS");

            entity.Property(e => e.Cgst)
                .HasColumnType("NUMBER(10,2)")
                .HasColumnName("CGST");
            entity.Property(e => e.Igst)
                .HasColumnType("NUMBER(10,2)")
                .HasColumnName("IGST");
            entity.Property(e => e.InvoiceNo)
                .HasMaxLength(14)
                .IsUnicode(false)
                .HasColumnName("INVOICE_NO");
            entity.Property(e => e.ItemDesc)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("ITEM_DESC");
            entity.Property(e => e.ItemSrno)
                .HasPrecision(2)
                .HasColumnName("ITEM_SRNO");
            entity.Property(e => e.Qty)
                .HasColumnType("NUMBER(8,2)")
                .HasColumnName("QTY");
            entity.Property(e => e.Rate)
                .HasColumnType("NUMBER(10,2)")
                .HasColumnName("RATE");
            entity.Property(e => e.Sgst)
                .HasColumnType("NUMBER(10,2)")
                .HasColumnName("SGST");
            entity.Property(e => e.TestingCharges)
                .HasColumnType("NUMBER(11,2)")
                .HasColumnName("TESTING_CHARGES");
        });

        modelBuilder.Entity<VTractionDistribution>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("V_TRACTION_DISTRIBUTION");

            entity.Property(e => e.CaseNo)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CASE_NO");
            entity.Property(e => e.Item)
                .HasMaxLength(66)
                .IsUnicode(false)
                .HasColumnName("ITEM");
            entity.Property(e => e.ItemDesc)
                .HasMaxLength(400)
                .IsUnicode(false)
                .HasColumnName("ITEM_DESC");
            entity.Property(e => e.QtyPassed)
                .HasColumnType("NUMBER(12,4)")
                .HasColumnName("QTY_PASSED");
            entity.Property(e => e.Uom)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("UOM");
        });

        modelBuilder.Entity<VendPoMa>(entity =>
        {
            entity.HasKey(e => new { e.CaseNo, e.MaNo, e.MaDt });

            entity.ToTable("VEND_PO_MA");

            entity.Property(e => e.CaseNo)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CASE_NO");
            entity.Property(e => e.MaNo)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MA_NO");
            entity.Property(e => e.MaDt)
                .HasColumnType("DATE")
                .HasColumnName("MA_DT");
            entity.Property(e => e.ApprovedBy)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("APPROVED_BY");
            entity.Property(e => e.ApprovedDatetime)
                .HasColumnType("DATE")
                .HasColumnName("APPROVED_DATETIME");
            entity.Property(e => e.Datetime)
                .HasColumnType("DATE")
                .HasColumnName("DATETIME");
            entity.Property(e => e.MaDesc)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("MA_DESC");
            entity.Property(e => e.MaStatus)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("MA_STATUS");
            entity.Property(e => e.NewPoValue)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("NEW_PO_VALUE");
            entity.Property(e => e.OldPoValue)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("OLD_PO_VALUE");
            entity.Property(e => e.PoDt)
                .HasColumnType("DATE")
                .HasColumnName("PO_DT");
            entity.Property(e => e.PoNo)
                .HasMaxLength(75)
                .IsUnicode(false)
                .HasColumnName("PO_NO");
            entity.Property(e => e.RlyCd)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("RLY_CD");
            entity.Property(e => e.RlyNonrly)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("RLY_NONRLY");
            entity.Property(e => e.UserId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("USER_ID");
        });

        modelBuilder.Entity<VendPoMaDetail>(entity =>
        {
            entity.HasKey(e => new { e.CaseNo, e.MaNo, e.MaDt, e.MaSno }).HasName("PK_VEND_PO_MA_DET");

            entity.ToTable("VEND_PO_MA_DETAIL");

            entity.Property(e => e.CaseNo)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CASE_NO");
            entity.Property(e => e.MaNo)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MA_NO");
            entity.Property(e => e.MaDt)
                .HasColumnType("DATE")
                .HasColumnName("MA_DT");
            entity.Property(e => e.MaSno)
                .HasPrecision(2)
                .HasColumnName("MA_SNO");
            entity.Property(e => e.ApprovedBy)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("APPROVED_BY");
            entity.Property(e => e.ApprovedDatetime)
                .HasColumnType("DATE")
                .HasColumnName("APPROVED_DATETIME");
            entity.Property(e => e.Datetime)
                .HasColumnType("DATE")
                .HasColumnName("DATETIME");
            entity.Property(e => e.MaDesc)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("MA_DESC");
            entity.Property(e => e.MaField)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("MA_FIELD");
            entity.Property(e => e.MaRemarks)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("MA_REMARKS");
            entity.Property(e => e.MaStatus)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("MA_STATUS");
            entity.Property(e => e.NewPoValue)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("NEW_PO_VALUE");
            entity.Property(e => e.OldPoValue)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("OLD_PO_VALUE");
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("USER_ID");

            entity.HasOne(d => d.VendPoMaMaster).WithMany(p => p.VendPoMaDetails)
                .HasForeignKey(d => new { d.CaseNo, d.MaNo, d.MaDt })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VEND_PO_MA_DET");
        });

        modelBuilder.Entity<VendPoMaMaster>(entity =>
        {
            entity.HasKey(e => new { e.CaseNo, e.MaNo, e.MaDt }).HasName("PK_VEND_PO_MA_MAST");

            entity.ToTable("VEND_PO_MA_MASTER");

            entity.Property(e => e.CaseNo)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CASE_NO");
            entity.Property(e => e.MaNo)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MA_NO");
            entity.Property(e => e.MaDt)
                .HasColumnType("DATE")
                .HasColumnName("MA_DT");
            entity.Property(e => e.PoDt)
                .HasColumnType("DATE")
                .HasColumnName("PO_DT");
            entity.Property(e => e.PoNo)
                .HasMaxLength(75)
                .IsUnicode(false)
                .HasColumnName("PO_NO");
            entity.Property(e => e.PoOrLetter)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("PO_OR_LETTER");
            entity.Property(e => e.PoSrc)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("PO_SRC");
            entity.Property(e => e.RlyCd)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("RLY_CD");
            entity.Property(e => e.RlyNonrly)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("RLY_NONRLY");
        });

        modelBuilder.Entity<VenderCallRegisterItemView1>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VENDER_CALL_REGISTER_ITEM_VIEW1");

            entity.Property(e => e.CallRecvDt)
                .HasColumnType("DATE")
                .HasColumnName("CALL_RECV_DT");
            entity.Property(e => e.CallSno)
                .HasPrecision(5)
                .HasColumnName("CALL_SNO");
            entity.Property(e => e.CaseNo)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CASE_NO");
            entity.Property(e => e.Consignee)
                .HasMaxLength(378)
                .IsUnicode(false)
                .HasColumnName("CONSIGNEE");
            entity.Property(e => e.CumQtyPrevOffered)
                .HasColumnType("NUMBER(12,4)")
                .HasColumnName("CUM_QTY_PREV_OFFERED");
            entity.Property(e => e.CumQtyPrevPassed)
                .HasColumnType("NUMBER(12,4)")
                .HasColumnName("CUM_QTY_PREV_PASSED");
            entity.Property(e => e.DelvDate)
                .IsUnicode(false)
                .HasColumnName("DELV_DATE");
            entity.Property(e => e.ItemDescPo)
                .HasMaxLength(400)
                .IsUnicode(false)
                .HasColumnName("ITEM_DESC_PO");
            entity.Property(e => e.ItemSrnoPo)
                .HasPrecision(4)
                .HasColumnName("ITEM_SRNO_PO");
            entity.Property(e => e.QtyDue)
                .HasColumnType("NUMBER(12,4)")
                .HasColumnName("QTY_DUE");
            entity.Property(e => e.QtyOrdered)
                .HasColumnType("NUMBER(12,4)")
                .HasColumnName("QTY_ORDERED");
            entity.Property(e => e.QtyPassed)
                .HasColumnType("NUMBER(12,4)")
                .HasColumnName("QTY_PASSED");
            entity.Property(e => e.QtyRejected)
                .HasColumnType("NUMBER(12,4)")
                .HasColumnName("QTY_REJECTED");
            entity.Property(e => e.QtyToInsp)
                .HasColumnType("NUMBER(12,4)")
                .HasColumnName("QTY_TO_INSP");
            entity.Property(e => e.Status)
                .HasMaxLength(6)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("STATUS");
        });

        modelBuilder.Entity<VenderCallRegisterItemView2>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VENDER_CALL_REGISTER_ITEM_VIEW2");

            entity.Property(e => e.CallRecvDt)
                .HasColumnType("DATE")
                .HasColumnName("CALL_RECV_DT");
            entity.Property(e => e.CallSno)
                .HasColumnType("NUMBER")
                .HasColumnName("CALL_SNO");
            entity.Property(e => e.CaseNo)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CASE_NO");
            entity.Property(e => e.Consignee)
                .HasMaxLength(378)
                .IsUnicode(false)
                .HasColumnName("CONSIGNEE");
            entity.Property(e => e.CumQtyPrevOffered)
                .HasColumnType("NUMBER")
                .HasColumnName("CUM_QTY_PREV_OFFERED");
            entity.Property(e => e.CumQtyPrevPassed)
                .HasColumnType("NUMBER")
                .HasColumnName("CUM_QTY_PREV_PASSED");
            entity.Property(e => e.DelvDate)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("DELV_DATE");
            entity.Property(e => e.ItemDescPo)
                .HasMaxLength(400)
                .IsUnicode(false)
                .HasColumnName("ITEM_DESC_PO");
            entity.Property(e => e.ItemSrnoPo)
                .HasPrecision(4)
                .HasColumnName("ITEM_SRNO_PO");
            entity.Property(e => e.QtyDue)
                .HasColumnType("NUMBER")
                .HasColumnName("QTY_DUE");
            entity.Property(e => e.QtyOrdered)
                .HasColumnType("NUMBER(12,4)")
                .HasColumnName("QTY_ORDERED");
            entity.Property(e => e.QtyPassed)
                .HasColumnType("NUMBER")
                .HasColumnName("QTY_PASSED");
            entity.Property(e => e.QtyRejected)
                .HasColumnType("NUMBER")
                .HasColumnName("QTY_REJECTED");
            entity.Property(e => e.QtyToInsp)
                .HasColumnType("NUMBER")
                .HasColumnName("QTY_TO_INSP");
            entity.Property(e => e.Status)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("STATUS");
        });

        modelBuilder.Entity<VendorCallPoDetailsView>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VENDOR_CALL_PO_DETAILS_VIEW");

            entity.Property(e => e.CaseNo)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CASE_NO");
            entity.Property(e => e.L5noPo)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("L5NO_PO");
            entity.Property(e => e.PoDt)
                .HasColumnType("DATE")
                .HasColumnName("PO_DT");
            entity.Property(e => e.PoNo)
                .HasMaxLength(75)
                .IsUnicode(false)
                .HasColumnName("PO_NO");
            entity.Property(e => e.PurchaserCd)
                .HasMaxLength(173)
                .IsUnicode(false)
                .HasColumnName("PURCHASER_CD");
            entity.Property(e => e.Rly)
                .HasMaxLength(28)
                .IsUnicode(false)
                .HasColumnName("RLY");
            entity.Property(e => e.RlyNonrly)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("RLY_NONRLY");
            entity.Property(e => e.VendCd)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("VEND_CD");
        });

        modelBuilder.Entity<VendorFeedback>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("VENDOR_FEEDBACK");

            entity.HasIndex(e => new { e.VendCd, e.RegionCode, e.MthyrPeriod }, "UK_VENDOR_FEEDBACK_NEW").IsUnique();

            entity.Property(e => e.Field1)
                .HasPrecision(1)
                .HasColumnName("FIELD_1");
            entity.Property(e => e.Field10)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("FIELD_10");
            entity.Property(e => e.Field2)
                .HasPrecision(1)
                .HasColumnName("FIELD_2");
            entity.Property(e => e.Field3)
                .HasPrecision(1)
                .HasColumnName("FIELD_3");
            entity.Property(e => e.Field4)
                .HasPrecision(1)
                .HasColumnName("FIELD_4");
            entity.Property(e => e.Field5)
                .HasPrecision(1)
                .HasColumnName("FIELD_5");
            entity.Property(e => e.Field6)
                .HasPrecision(1)
                .HasColumnName("FIELD_6");
            entity.Property(e => e.Field7)
                .HasPrecision(1)
                .HasColumnName("FIELD_7");
            entity.Property(e => e.Field8)
                .HasPrecision(1)
                .HasColumnName("FIELD_8");
            entity.Property(e => e.Field9)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("FIELD_9");
            entity.Property(e => e.MthyrPeriod)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("MTHYR_PERIOD");
            entity.Property(e => e.RegionCode)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("REGION_CODE");
            entity.Property(e => e.VendCd)
                .HasPrecision(6)
                .HasColumnName("VEND_CD");
        });

        modelBuilder.Entity<ViewGetmanufvend>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VIEW_GETMANUFVEND");

            entity.Property(e => e.VendAdd1)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("VEND_ADD1");
            entity.Property(e => e.VendCd)
                .HasPrecision(6)
                .HasColumnName("VEND_CD");
            entity.Property(e => e.VendContactPer1)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("VEND_CONTACT_PER_1");
            entity.Property(e => e.VendContactTel1)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("VEND_CONTACT_TEL_1");
            entity.Property(e => e.VendEmail)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("VEND_EMAIL");
            entity.Property(e => e.VendName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("VEND_NAME");
            entity.Property(e => e.VendStatus)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("VEND_STATUS");
            entity.Property(e => e.VendStatusFr)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("VEND_STATUS_FR");
            entity.Property(e => e.VendStatusTo)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("VEND_STATUS_TO");
        });

        modelBuilder.Entity<ViewGetvendor>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VIEW_GETVENDOR");

            entity.Property(e => e.VendCd)
                .HasPrecision(6)
                .HasColumnName("VEND_CD");
            entity.Property(e => e.VendName)
                .HasMaxLength(305)
                .IsUnicode(false)
                .HasColumnName("VEND_NAME");
        });

        modelBuilder.Entity<ViewLaboratory>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VIEW_LABORATORY");

            entity.Property(e => e.City)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CITY");
            entity.Property(e => e.LabAddress)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("LAB_ADDRESS");
            entity.Property(e => e.LabId)
                .HasPrecision(6)
                .HasColumnName("LAB_ID");
            entity.Property(e => e.LabName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("LAB_NAME");
        });

        modelBuilder.Entity<ViewPomasterlist>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VIEW_POMASTERLIST");

            entity.Property(e => e.CaseNo)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CASE_NO");
            entity.Property(e => e.ConsigneeSName)
                .HasMaxLength(132)
                .IsUnicode(false)
                .HasColumnName("CONSIGNEE_S_NAME");
            entity.Property(e => e.PoDt)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("PO_DT");
            entity.Property(e => e.PoNo)
                .HasMaxLength(75)
                .IsUnicode(false)
                .HasColumnName("PO_NO");
            entity.Property(e => e.RealCaseNo)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("REAL_CASE_NO");
            entity.Property(e => e.Remarks)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("REMARKS");
            entity.Property(e => e.RlyCd)
                .HasMaxLength(68)
                .IsUnicode(false)
                .HasColumnName("RLY_CD");
            entity.Property(e => e.VendCd)
                .HasPrecision(6)
                .HasColumnName("VEND_CD");
            entity.Property(e => e.VendName)
                .HasMaxLength(205)
                .IsUnicode(false)
                .HasColumnName("VEND_NAME");
        });

        modelBuilder.Entity<ViewVoucherList>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VIEW_VOUCHER_LIST");

            entity.Property(e => e.AccDesc)
                .HasMaxLength(81)
                .IsUnicode(false)
                .HasColumnName("ACC_DESC");
            entity.Property(e => e.Amount)
                .HasColumnType("NUMBER(12,2)")
                .HasColumnName("AMOUNT");
            entity.Property(e => e.BankName)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("BANK_NAME");
            entity.Property(e => e.BpoName)
                .HasMaxLength(310)
                .IsUnicode(false)
                .HasColumnName("BPO_NAME");
            entity.Property(e => e.CaseNo)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CASE_NO");
            entity.Property(e => e.ChqDt)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("CHQ_DT");
            entity.Property(e => e.ChqNo)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("CHQ_NO");
            entity.Property(e => e.Narration)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NARRATION");
            entity.Property(e => e.Sno)
                .HasPrecision(4)
                .HasColumnName("SNO");
            entity.Property(e => e.VchrNo)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("VCHR_NO");
        });
        modelBuilder.HasSequence("AUDIT_SEQ");
        modelBuilder.HasSequence("IBS_APPDOCUMENT_SEQ");
        modelBuilder.HasSequence("IBS_DOCUMENT_SEQ");
        modelBuilder.HasSequence("IBS_DOCUMENTCATEGORY_SEQ");
        modelBuilder.HasSequence("MICROSOFTSEQDTPROPERTIES");
        modelBuilder.HasSequence("RITESPOSEQ");
        modelBuilder.HasSequence("ROLES_SEQ");
        modelBuilder.HasSequence("T02_USERS_SEQ");
        modelBuilder.HasSequence("T04_UOM_SEQ");
        modelBuilder.HasSequence("T05_VENDOR_SEQ");
        modelBuilder.HasSequence("T07_RITES_DESIG_SEQ");
        modelBuilder.HasSequence("T13_PO_MASTER_SEQ");
        modelBuilder.HasSequence("T45_CLAIM_MASTER_SEQ");
        modelBuilder.HasSequence("T46_CLAIM_DETAIL_SEQ");
        modelBuilder.HasSequence("T47_IE_WORK_PLAN_SEQ");
        modelBuilder.HasSequence("T50_LAB_REGISTER_SEQ");
        modelBuilder.HasSequence("T53_VIGILANCE_CASES_MASTER_SEQ");
        modelBuilder.HasSequence("T58_CLIENT_CONTACT_SEQ");
        modelBuilder.HasSequence("T65_LABORATORY_MASTER_SEQ");
        modelBuilder.HasSequence("T65_LABORATORY_MASTER_SEQ_1").IncrementsBy(135);
        modelBuilder.HasSequence("T65_LABORATORY_MASTER_SEQ_2");
        modelBuilder.HasSequence("T91_RAILWAYS_SEQ");
        modelBuilder.HasSequence("T94_BANK_SEQ");
        modelBuilder.HasSequence("T96_MESSAGES_SEQ");
        modelBuilder.HasSequence("TBLEXCEPTION_SEQ");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
