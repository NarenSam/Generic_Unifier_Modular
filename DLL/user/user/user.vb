Public Class user
    Public ssmartid As Integer
    Public ssname As String
    Public sslocation As String
    Public sstatus As Boolean
    Public aaction As String
    Public aage As Integer
    Public ccategory As Integer
    Public ppriority As Integer
    Public Property Action As String
        Get
            Return aaction
        End Get
        Set(ByVal value As String)
            aaction = value
        End Set
    End Property
    
    Public Property Sname As String
        Get
            Return ssname
        End Get
        Set(ByVal value As String)
            ssname = value
        End Set
    End Property

    Public Property Smartid As Integer
        Get
            Return ssmartid
        End Get
        Set(ByVal value As Integer)
            ssmartid = value
        End Set
    End Property
    Public Property Slocation As String
        Get
            Return sslocation
        End Get
        Set(ByVal value As String)
            sslocation = value
        End Set
    End Property

    Public Property Status As Boolean
        Get
            Return sstatus
        End Get
        Set(ByVal value As Boolean)
            sstatus = value
        End Set
    End Property

    Public Property Age As Integer
        Get
            Return aage
        End Get
        Set(ByVal value As Integer)
            aage = value
        End Set
    End Property

    Public Property Category As Integer
        Get
            Return ccategory
        End Get
        Set(ByVal value As Integer)
            ccategory = value
        End Set
    End Property

    Public Property Priority As Integer
        Get
            Return ppriority
        End Get
        Set(ByVal value As Integer)
            ppriority = value
        End Set
    End Property

End Class
