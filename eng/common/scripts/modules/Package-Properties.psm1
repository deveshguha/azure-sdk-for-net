# Helper functions for retireving useful information from azure-sdk-for-* repo
# Example Use : Import-Module .\eng\common\scripts\modules
class PackageProps
{
    [string]$pkgName
    [AzureEngSemanticVersion]$pkgVersion
    [string]$pkgDirectoryPath
    [string]$pkgReadMePath
    [string]$pkgChangeLogPath

    PackageProps(
        [string]$pkgName,
        [string]$pkgVersion,
        [string]$pkgDirectoryPath
    )
    {
        $this.pkgName = $pkgName
        $this.pkgVersion = [AzureEngSemanticVersion]::new($pkgVersion)
        $this.pkgDirectoryPath = $pkgDirectoryPath

        if (Test-Path (Join-Path $pkgDirectoryPath "README.md"))
        {
            $this.pkgReadMePath = Join-Path $pkgDirectoryPath "README.md"
        } 
        else
        {
            $this.pkgReadMePath = $null
        }

        if (Test-Path (Join-Path $pkgDirectoryPath "CHANGELOG.md"))
        {
            $this.pkgChangeLogPath = Join-Path $pkgDirectoryPath "CHANGELOG.md"
        } 
        else
        {
            $this.pkgChangeLogPath = $null
        }
    }
}

Install-Module -Name powershell-yaml -RequiredVersion 0.4.1 -Force -Scope CurrentUser

function Extract-PkgProps ($pkgPath, $pkgName)
{
    if ($Language -eq "net")
    {
        return Extract-DotNetPkgProps -pkgPath $pkgPath -pkgName $pkgName 
    }
    if ($Language -eq "java")
    {
        return Extract-JavaPkgProps -pkgPath $pkgPath -pkgName $pkgName 
    }
    if ($Language -eq "js")
    {
        return Extract-JsPkgProps -pkgPath $pkgPath -pkgName $pkgName 
    }
    if ($Language -eq "python")
    {
        return Extract-PythonPkgProps -pkgPath $pkgPath -pkgName $pkgName 
    }
}

function Extract-DotNetPkgProps ($pkgPath, $pkgName)
{
    $projectPath = Join-Path $pkgPath "src" "$pkgName.csproj"
    if (Test-Path $projectPath)
    {
        $projectData = New-Object -TypeName XML
        $projectData.load($projectPath)
        $pkgVersion = Select-XML -Xml $projectData -XPath '/Project/PropertyGroup/Version'
        return [PackageProps]::new($pkgName, $pkgVersion, $pkgPath)
    } 
    else 
    {
        return $null
    }
}

function Extract-JsPkgProps ($pkgPath, $pkgName)
{
    $projectPath = Join-Path $pkgPath "package.json"
    if (Test-Path $projectPath)
    {
        $projectJson = Get-Content $projectPath | ConvertFrom-Json
        $jsStylePkgName = $pkgName.replace("azure-", "@azure/")
        if ($projectJson.name -eq "$jsStylePkgName")
        {
            return [PackageProps]::new($projectJson.name, $projectJson.version, $pkgPath)
        }
    }
    return $null
}

function Extract-PythonPkgProps ($pkgPath, $pkgName)
{
    $pkgName = $pkgName.Replace('_', '-')

    if (Test-Path (Join-Path $pkgPath "setup.py"))
    {
        $setupLocation = $pkgPath.Replace('\','/')
        pushd $RepoRoot
        $setupProps = (python -c "import scripts.devops_tasks.common_tasks; obj=scripts.devops_tasks.common_tasks.parse_setup('$setupLocation'); print('{0},{1}'.format(obj[0], obj[1]));") -split ","
        popd
        if (($setupProps -ne $null) -and ($setupProps[0] -eq $pkgName))
        {
            return [PackageProps]::new($setupProps[0], $setupProps[1], $pkgPath)
        }
    }
    return $null
}

function Extract-JavaPkgProps ($pkgPath, $pkgName)
{
    $projectPath = Join-Path $pkgPath "pom.xml"

    if (Test-Path $projectPath)
    {
        $projectData = New-Object -TypeName XML
        $projectData.load($projectPath)
        $projectPkgName = $projectData.project.artifactId
        $pkgVersion = $projectData.project.version

        if ($projectPkgName -eq $pkgName)
        {
            return [PackageProps]::new($pkgName, $pkgVersion.ToString(), $pkgPath)
        }
    }
    return $null
}

# Takes package name and service Name
# Returns important properties of the package as related to the language repo
# Returns a PS Object with properties @ { pkgName, pkgVersion, pkgDirectoryPath, pkgReadMePath, pkgChangeLogPath }
# Note: python is required for parsing python package properties.
function Get-PkgProperties
{
    Param
    (
        [Parameter(Mandatory=$true)]
        [string]$PackageName,
        [Parameter(Mandatory=$true)]
        [string]$ServiceName,
        [Parameter(Mandatory=$true)]
        [ValidateSet("net","java","js","python")]
        [string]$Language,
        [string]$RepoRoot="${PSScriptRoot}/../../../.."
    )

    $pkgDirectoryName = $null
    $pkgDirectoryPath = $null
    $serviceDirectoryPath = Join-Path $RepoRoot "sdk" $ServiceName
    if (!(Test-Path $serviceDirectoryPath))
    {
        Write-Error "Service Directory $ServiceName does not exist"
        exit 1
    }

    $directoriesPresent = Get-ChildItem $serviceDirectoryPath -Directory

    foreach ($directory in $directoriesPresent)
    {
        $pkgDirectoryPath = Join-Path $serviceDirectoryPath $directory.Name
        $pkgProps = Extract-PkgProps -pkgPath $pkgDirectoryPath -pkgName $PackageName
        if ($pkgProps -ne $null)
        {
            return $pkgProps
        }
    }
    Write-Error "Failed to retrive Properties for $PackageName"
}

# Takes ServiceName, Language, and Repo Root Directory
# Returns important properties for each package in the specified service, or entire repo if the serviceName is not specified
# Returns an array of PS Object with properties @ { pkgName, pkgVersion, pkgDirectoryPath, pkgReadMePath, pkgChangeLogPath }
function Get-AllPkgProperties
{
    Param
    (
        [Parameter(Mandatory=$true)]
        [ValidateSet("net","java","js","python")]
        [string]$Language,
        [string]$RepoRoot="${PSScriptRoot}/../../../..",
        [string]$ServiceName=$null
    )

    if ([string]::IsNullOrEmpty($ServiceName))
    {
        $searchDir = Join-Path $RepoRoot "sdk"

        foreach ($dir in (Get-ChildItem $searchDir -Directory))
        {
            $serviceDir = Join-Path $searchDir $dir.Name

            if (Test-Path (Join-Path $serviceDir "ci.yml"))
            {
                $activePkgList = Get-PkgListFromYml -ciYmlPath (Join-Path $serviceDir "ci.yml")
                return Operate-OnPackages -activePkgList $activePkgList -serviceName $dir.Name -language $Language -repoRoot $RepoRoot
            }
        }
    } 
    else
    {
        $serviceDir = Join-Path $RepoRoot "sdk" $ServiceName
        if (Test-Path (Join-Path $serviceDir "ci.yml"))
        {
            $activePkgList = Get-PkgListFromYml -ciYmlPath (Join-Path $serviceDir "ci.yml")
            return Operate-OnPackages -activePkgList $activePkgList -serviceName $ServiceName -language $Language -repoRoot $RepoRoot
        }
    }
}

function Operate-OnPackages ($activePkgList, $serviceName, $repoRoot, $language)
{
    $pkgsProps = @()
    foreach ($pkg in $activePkgList)
    {
        $pkgProps = Get-PkgProperties -PackageName $pkg["name"] -ServiceName $serviceName -Language $language -RepoRoot $repoRoot
        $pkgsProps += $pkgProps
    }
    return $pkgsProps
}

function Get-PkgListFromYml ($ciYmlPath)
{
    $ciYmlContent = Get-Content $ciYmlPath -Raw
    $ciYmlObj = ConvertFrom-Yaml $ciYmlContent -Ordered
    $artifactsInCI = $ciYmlObj["stages"][0]["parameters"]["Artifacts"]

    if ($artifactsInCI -eq $null)
    {
        Write-Error "Failed to retrive package names in ci $ciYmlPath"
        exit 1
    } 
    else
    {
        return $artifactsInCI
    }
}

Export-ModuleMember -Function 'Get-PkgProperties'
Export-ModuleMember -Function 'Get-AllPkgProperties'