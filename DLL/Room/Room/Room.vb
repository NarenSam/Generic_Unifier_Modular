Public Class Room

    Public ssmartid As Integer
    Public ssname As String
    Public sslocation As String
    Public sstatus As Boolean
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
End Class
