//******************************************************************************
//
// Copyright (c) 2018 Microsoft Corporation. All rights reserved.
//
// This code is licensed under the MIT License (MIT).
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
//******************************************************************************

#include "StdAfx.h"
#include <oleacc.h>
#include <windowsx.h>
#include <regex>
#include "UiTreeWalk.h"
#include "ControlTypeId.h"

const int MinDist = 50;
static const WCHAR* c_chNodeFormat = L"/%s[LocalizedControlType=\"%s\"][ClassName=\"%s\"][Name=\"%s\"][AutomationId=\"%s\"][x=%d][y=%d][width=%d][height=%d][lx=%d][ly=%d][position()=%s][RuntimeId=\"%s\"]\n";
static const size_t MaxNameLength = 64;

WCHAR UiTreeWalk::s_chBuffer[BUFFERSIZE];
HANDLE UiTreeWalk::s_hEventPathReady = NULL;
HANDLE UiTreeWalk::s_hUiTreeWalkerThread = NULL;
DWORD UiTreeWalk::s_dwUiTreeWalkerThread = 0;
const RECT rectZero{ 0,0,0,0 };
RECT UiTreeWalk::s_rectTargetElement{ 0,0,0,0 };

#ifdef _DEBUG
static const DWORD dwWaitUiWalk = INFINITE;
#else
static const DWORD dwWaitUiWalk = 30000;
#endif

bool XmlEncode(std::wstring& data, int nMaxCount)
{
    if ((int)data.size() > nMaxCount)
    {
        data = data.substr(0, nMaxCount);
    }

    size_t nCount = 0;
    std::wstring buffer;
    buffer.reserve(data.size());
    for (size_t pos = 0; pos != data.size(); ++pos)
    {
        if (data[pos] == L'\n' || data[pos] == L'\r')
        {
            nCount = pos;
            break;
        }
        else if (data[pos] == L'&')
        {
            buffer.append(L"&amp;");
        }
        else if (data[pos] == L'\"')
        {
            buffer.append(L"&quot;");
        }
        else if (data[pos] == L'\'')
        {
            buffer.append(L"&apos;");
        }
        else if (data[pos] == L'<')
        {
            buffer.append(L"&lt;");
        }
        else if (data[pos] == L'>')
        {
            buffer.append(L"&gt;");
        }
        else if (data[pos] == L'\\')
        {
            buffer.append(L"\\\\");
        }
        else
        {
            buffer.append(&data[pos], 1);
        }
    }

    data.swap(buffer);

    return nCount > 0;
}

void UiTreeWalk::Init()
{
    if (UiTreeWalk::s_hEventPathReady == NULL)
    {
        UiTreeWalk::s_hEventPathReady = CreateEvent(NULL, TRUE, FALSE, NULL);
    }

    if (s_hUiTreeWalkerThread == NULL)
    {
        s_hUiTreeWalkerThread = CreateThread(NULL,
            0,
            (LPTHREAD_START_ROUTINE)(UiTreeWalk::UiTreeWalkerThreadProc),
            (void*)NULL,
            0,
            &s_dwUiTreeWalkerThread);

        if (s_hUiTreeWalkerThread == NULL || s_dwUiTreeWalkerThread == 0)
        {
            throw L"Failed to create UiTreeWalk thread";
        }
        else
        {
            if (UiTreeWalk::s_hEventPathReady != NULL && WAIT_OBJECT_0 != WaitForSingleObject(UiTreeWalk::s_hEventPathReady, 30000))
            {
                throw L"UiTreeWalk thread is not responding";
            }
        }
    }
}

void UiTreeWalk::UnInit()
{
    PostThreadMessage(UiTreeWalk::s_dwUiTreeWalkerThread, WM_QUIT, 0, 0);

    if (WAIT_OBJECT_0 == WaitForSingleObject(UiTreeWalk::s_hEventPathReady, 10000))
    {

    }

    if (UiTreeWalk::s_hEventPathReady != NULL)
    {
        CloseHandle(UiTreeWalk::s_hEventPathReady);
        UiTreeWalk::s_hEventPathReady = NULL;
    }

    if (s_hUiTreeWalkerThread != NULL)
    {
        CloseHandle(s_hUiTreeWalkerThread);
        s_hUiTreeWalkerThread = NULL;
    }
}

HRESULT UiTreeWalk::AppendUiAttributes(long left, long top, IUIAutomationElement* pCurUia, long nPos, std::wstring& wstr)
{
    SAFEARRAY *rid;
    REQUIRE_SUCCESS_HR(pCurUia->GetRuntimeId(&rid));
    LONG lbound;
    REQUIRE_SUCCESS_HR(SafeArrayGetLBound(rid, 1, &lbound));
    LONG ubound;
    REQUIRE_SUCCESS_HR(SafeArrayGetUBound(rid, 1, &ubound));

    CComBSTR bstrRuntimeId;
    LONG runtimeId = 0;
    WCHAR temp[16];
    for (LONG i = lbound; i <= ubound; i++)
    {
        REQUIRE_SUCCESS_HR(SafeArrayGetElement(rid, &i, &runtimeId));
        _ltow_s(runtimeId, temp, 10);
        REQUIRE_SUCCESS_HR(bstrRuntimeId.Append(temp));
        
        if (i < ubound)
        {
            REQUIRE_SUCCESS_HR(bstrRuntimeId.Append(L"."));
        }
    }

    REQUIRE_SUCCESS_HR(SafeArrayDestroy(rid));

    if (bstrRuntimeId == NULL)
    {
        wsprintf(temp, L"%lu", reinterpret_cast<std::uintptr_t>(pCurUia));
        bstrRuntimeId = temp;
    }

    CComPtr<IUIAutomationElement> spUia(pCurUia);
    auto it = cachedAutoElements.find(bstrRuntimeId);
    if (it == cachedAutoElements.end())
    {
        cachedAutoElements.insert(std::make_pair(bstrRuntimeId, spUia));
    }
    else
    {
        it->second = spUia;
    }

    CComBSTR bstrClass;
    REQUIRE_SUCCESS_HR(pCurUia->get_CurrentClassName(&bstrClass));
    bstrClass = bstrClass == NULL ? L"" : bstrClass;
    std::wstring shortClass(bstrClass, SysStringLen(bstrClass));
    bool bStartWith = XmlEncode(shortClass, MaxNameLength);
    if (bStartWith == true || shortClass.length() >= MaxNameLength)
    {
        shortClass.insert(0, L"starts-with:");
    }

    CComBSTR bstrName;
    REQUIRE_SUCCESS_HR(pCurUia->get_CurrentName(&bstrName));
    bstrName = bstrName == NULL ? L"" : bstrName;

    std::wstring shortName(bstrName, SysStringLen(bstrName));
    bStartWith = XmlEncode(shortName, MaxNameLength);

    if (bStartWith == true || shortName.length() >= MaxNameLength)
    {
        shortName.insert(0, L"starts-with:");
    }

    CComBSTR bstrCtrlType;
    REQUIRE_SUCCESS_HR(pCurUia->get_CurrentLocalizedControlType(&bstrCtrlType));
    bstrCtrlType = bstrCtrlType == NULL ? L"" : bstrCtrlType;

    CONTROLTYPEID cid;
    REQUIRE_SUCCESS_HR(pCurUia->get_CurrentControlType(&cid));

    CComBSTR bstrProgrammaticName;
    if (cid >= UIA_ButtonControlTypeId && UIA_ButtonControlTypeId <= UIA_AppBarControlTypeId)
    {
        REQUIRE_SUCCESS_HR(bstrProgrammaticName.Append(gc_controlTypesTable[cid - UIA_ButtonControlTypeId].pszName));
    }

    // CurrentLocalizedControlType can be empty: Cortana set reminder Time button's parent
    if (bstrProgrammaticName.Length() == 0)
    {
        bstrProgrammaticName = L"Unknown";
    }

    CComBSTR bstrAutoId;
    REQUIRE_SUCCESS_HR(pCurUia->get_CurrentAutomationId(&bstrAutoId));
    bstrAutoId = bstrAutoId == NULL ? L"" : bstrAutoId;

    std::wstring shortAutoId(bstrAutoId, SysStringLen(bstrAutoId));
    bStartWith = XmlEncode(shortAutoId, MaxNameLength);

    if (bStartWith == true || shortAutoId.length() >= MaxNameLength)
    {
        shortAutoId.insert(0, L"starts-with:");
    }

    RECT rect;
    REQUIRE_SUCCESS_HR(pCurUia->get_CurrentBoundingRectangle(&rect));

    WCHAR chPos[16];
    if (nPos <= 0)
    {
        wsprintf(chPos, L"");
    }
    else
    {
        wsprintf(chPos, L"%d", nPos + 1); // xpath index starts at 1
    } 

    wsprintf(UiTreeWalk::s_chBuffer,
        c_chNodeFormat,
        bstrProgrammaticName.m_str,
        bstrCtrlType.m_str,
        shortClass.c_str(),
        shortName.c_str(),
        shortAutoId.c_str(),
        rect.left,
        rect.top,
        rect.right - rect.left,
        rect.bottom - rect.top,
        left - rect.left,
        top - rect.top,
        chPos,
        bstrRuntimeId.m_str);

    if (wcslen(UiTreeWalk::s_chBuffer) > 0)
    {
        wstr.append(UiTreeWalk::s_chBuffer);
    }

    return S_OK;
}

long UiTreeWalk::GetUiXPath(_In_ long left, _In_ long top, _Out_writes_to_(nMaxCount, return) LPWSTR lpUiPath, _In_ long nMaxCount)
{
    ResetEvent(UiTreeWalk::s_hEventPathReady);

    PostThreadMessage(UiTreeWalk::s_dwUiTreeWalkerThread, msgGetXPath, 0, (DWORD)(left << 16 | top));

    if (WAIT_OBJECT_0 == WaitForSingleObject(UiTreeWalk::s_hEventPathReady, dwWaitUiWalk))
    {
        swprintf_s(lpUiPath, nMaxCount, L"%s", s_chBuffer);
    }
    else
    {
        swprintf_s(lpUiPath, nMaxCount, L"");
    }

    return (long)wcsnlen_s(lpUiPath, nMaxCount);
}

UiTreeWalk::UiTreeWalk() noexcept
{
    m_hdcDeskTop = GetDC(GetDesktopWindow());
    SetROP2(m_hdcDeskTop, R2_XORPEN);
    m_nHighlightTimer = 0;
    m_hWndTarget = NULL;
    m_pt = { 0,0 };
}

UiTreeWalk::~UiTreeWalk()
{
    if (m_hdcDeskTop != nullptr)
    {
        ReleaseDC(GetDesktopWindow(), m_hdcDeskTop);
        m_hdcDeskTop = nullptr;
    }
}

HRESULT UiTreeWalk::InitAutomation()
{
    if (m_spAutomation == nullptr)
    {
        REQUIRE_SUCCESS_HR(CoCreateInstance(__uuidof(CUIAutomation8), nullptr,
            CLSCTX_INPROC_SERVER,
            __uuidof(IUIAutomation),
            reinterpret_cast<void**>(&m_spAutomation)));

        REQUIRE_SUCCESS_HR(m_spAutomation->CreateTrueCondition(&m_spFindAllCondition)); // Remote Assistance Settings Cancel button

        REQUIRE_SUCCESS_HR(m_spAutomation->get_RawViewWalker(&m_spTreeWalker));

        return S_OK;
    }

    return S_FALSE;
}

void UiTreeWalk::UnInitAutomation()
{
    if (m_spFindAllCondition != nullptr)
    {
        m_spFindAllCondition.Release();
    }

    if (m_spTreeWalker != nullptr)
    {
        m_spTreeWalker.Release();
    }

    if (m_spAutomation != nullptr)
    {
        m_spAutomation.Release();
    }
}

HRESULT UiTreeWalk::ElementContainingPoint(long left, long top, IUIAutomationElement*pCur, IUIAutomationElement** ppElem)
{
    if (CancelCurrentTask())
    {
        return S_FALSE;
    }

    RECT rect;
    if (SUCCEEDED(pCur->get_CurrentBoundingRectangle(&rect)))
    {
        if (memcmp(&rect, &rectZero, sizeof(RECT)) != 0)
        {
            if (left < rect.left || left > rect.right)
                return S_FALSE;

            if (top < rect.top || top > rect.bottom)
                return S_FALSE;
        }
    }

    CComPtr<IUIAutomationElementArray> spChildren;
    REQUIRE_SUCCESS_HR(pCur->FindAll(TreeScope::TreeScope_Children, m_spFindAllCondition, &spChildren.p));

    if (spChildren != nullptr)
    {
        int count = 0;
        REQUIRE_SUCCESS_HR(spChildren->get_Length(&count));

        for (int i = 0; i < count; i++)
        {
            CComPtr<IUIAutomationElement> spChild;
            REQUIRE_SUCCESS_HR(spChildren->GetElement(i, &spChild.p));
            if (spChild != nullptr && S_FALSE != ElementContainingPoint(left, top, spChild, ppElem))
            {
                return S_OK;
            }
        }
    }

    spChildren.Release();
    REQUIRE_SUCCESS_HR(pCur->FindAll(TreeScope::TreeScope_Subtree, m_spFindAllCondition, &spChildren.p));
    if (spChildren == nullptr)
        return S_FALSE;

    int countAll = 0;
    REQUIRE_SUCCESS_HR(spChildren->get_Length(&countAll));

    double areaMin = 1E12;
    for (int i = 0; i < countAll; i++)
    {
        CComPtr<IUIAutomationElement> spChild;
        REQUIRE_SUCCESS_HR(spChildren->GetElement(i, &spChild.p));
        {
            if (spChild != nullptr && SUCCEEDED(spChild->get_CurrentBoundingRectangle(&rect)))
            {
                if (rect.left <= left && left <= rect.right)
                {
                    if (rect.top <= top && top <= rect.bottom)
                    {
                        double area = ((double)rect.right - (double)rect.left) * ((double)rect.bottom - (double)rect.top);
                        if (area < areaMin)
                        {
                            areaMin = area;
                            *ppElem = spChild;
                        }
                    }
                }
            }
        }
    }

    if (*ppElem != NULL)
    {
        (*ppElem)->AddRef();
        return S_OK;
    }
    else
    {
        return E_FAIL;
    }
}

HRESULT UiTreeWalk::PathFromRootToTarget(long left, long top)
{
    m_pt = { left,top };

    m_hWndTarget = WindowFromPoint(m_pt);

    BOOL bStartFromRoot = TRUE;

    HWND hRoot = GetAncestor(m_hWndTarget, GA_ROOT);
    if (hRoot != NULL)
    {
        WCHAR chClassName[64]{ 0 };
        GetClassName(hRoot, chClassName, ARRAYSIZE(chClassName));
        if (wcslen(chClassName) > 0)
        {
            if (wcsncmp(L"Chrome_WidgetWin", chClassName, 16) == 0
                || wcsncmp(L"MozillaWindowClass", chClassName, 18) == 0)
            {
                // Chrome and Firefox 
                return PathFromTargetToRoot(left, top);
            }

            if (wcsncmp(L"Progman", chClassName, 7) == 0
                || wcsncmp(L"IEFrame", chClassName, 7) == 0
                || wcsncmp(L"SysListView32", chClassName, 13) == 0)
            {
                // desktop icons or IE11
                bStartFromRoot = FALSE;
            }
        }
    }

    CComPtr<IUIAutomationElement> spRoot;
    REQUIRE_SUCCESS_HR(m_spAutomation->GetRootElement(&spRoot.p));

    CComPtr<IUIAutomationElement> spTarget;
    REQUIRE_SUCCESS_HR(m_spAutomation->ElementFromPoint(m_pt, &spTarget.p));

    CComPtr<IUIAutomationElement> spStart;
    if (bStartFromRoot == FALSE)
    {
        // starting from top window is much faster
        REQUIRE_SUCCESS_HR(m_spAutomation->ElementFromHandle(hRoot, &spStart.p));
    }
    else
    {
        REQUIRE_SUCCESS_HR(spRoot.CopyTo(&spStart.p));
    }

    // ElementFromPoint may return RECT {0,0,0,0} like UIs on Edge F12
    if (SUCCEEDED(spTarget->get_CurrentBoundingRectangle(&s_rectTargetElement)))
    {
        if (memcmp(&s_rectTargetElement, &rectZero, sizeof(RECT)) == 0)
        {
            CComPtr<IUIAutomationElement> TempTarget;
            if (FAILED(ElementContainingPoint(left, top, spStart, &TempTarget.p)))
            {
                CComPtr<IUIAutomationElement> spParent;
                m_spTreeWalker->GetParentElement(spTarget, &spParent.p);
                spTarget.Release();
                spTarget = spParent.Detach();
            }
            else
            {
                spTarget.Release();
                spTarget = TempTarget.Detach();
            }
        }
    }

    std::wstring uiPath;
    HRESULT hr = GetChildUiNode(left, top, spStart, spTarget, uiPath);
    if (hr == S_OK)
    {
        AppendUiAttributes(left, top, spStart, 0, uiPath);

        if (bStartFromRoot == FALSE)
        {
            AppendUiAttributes(left, top, spRoot, 0, uiPath);
        }

        swprintf_s(s_chBuffer, BUFFERSIZE, L"%s", uiPath.c_str());

        if (SUCCEEDED(spTarget->get_CurrentBoundingRectangle(&s_rectTargetElement)))
        {
            DrawHighlight();
            m_nHighlightTimer = SetTimer(NULL, 0, 500, NULL);
        }
    }
    else
    {
        PathFromTargetToRoot(left, top);
    }

    return S_OK;
}

HRESULT UiTreeWalk::GetChildUiNode(long left, long top, IUIAutomationElement* pCur, IUIAutomationElement* pFromPoint, std::wstring& uiPath)
{
    if (CancelCurrentTask())
    {
        return S_FALSE;
    }

    BOOL bFoundTarget = FALSE;
    m_spAutomation->CompareElements(pFromPoint, pCur, &bFoundTarget);
    if (bFoundTarget)
    {
        return S_OK;
    }

    CComPtr<IUIAutomationElementArray> spChildren;
    REQUIRE_SUCCESS_HR(pCur->FindAll(TreeScope::TreeScope_Children, m_spFindAllCondition, &spChildren.p));

    if (spChildren != nullptr)
    {
        int count = 0;
        REQUIRE_SUCCESS_HR(spChildren->get_Length(&count));

        for (int i = 0; i < count; i++)
        {
            CComPtr<IUIAutomationElement> spChild;
            REQUIRE_SUCCESS_HR(spChildren->GetElement(i, &spChild.p));

            //Ensure child is not same as parent - bing bottom tiles on IE11 cause stack overflow
            m_spAutomation->CompareElements(spChild, pCur, &bFoundTarget);

            if (spChild != nullptr && bFoundTarget == FALSE)
            {
                if (GetChildUiNode(left, top, spChild, pFromPoint, uiPath) == S_OK)
                {
                    AppendUiAttributes(left, top, spChild, i, uiPath);
                    return S_OK;
                }
            }
        }
    }

    return S_FALSE;
}

void UiTreeWalk::InvalidateTargetRect()
{
    if (m_nHighlightTimer != 0)
    {
        KillTimer(NULL, m_nHighlightTimer);
        m_nHighlightTimer = 0;
    }

    if (m_hWndTarget != nullptr)
    {
        InvalidateRect(m_hWndTarget, NULL, TRUE);
        m_hWndTarget = nullptr;
    }
}

void DrawYellowHighlightRect(HDC hdc, RECT rc)
{
    HBRUSH hBr = SelectBrush(hdc, GetStockBrush(NULL_BRUSH));

    int YellowInflat = -2;
    if (rc.bottom - rc.top < 30)
    {
        YellowInflat = 2;
    }

    HPEN hpYellow = CreatePen(PS_SOLID, 3, RGB(255, 255, 32));
    hpYellow = SelectPen(hdc, hpYellow);
    InflateRect(&rc, YellowInflat, YellowInflat);
    Rectangle(hdc, rc.left, rc.top, rc.right, rc.bottom);

    DeletePen(hpYellow);
    SelectBrush(hdc, hBr);
}

void UiTreeWalk::DrawHighlight()
{
    POINT pt;
    GetPhysicalCursorPos(&pt);

    if (abs(pt.x - m_pt.x) > MinDist || abs(pt.y - m_pt.y) > MinDist)
    {
        InvalidateTargetRect();
        return;
    }

    DrawYellowHighlightRect(m_hdcDeskTop, s_rectTargetElement);
}

BOOL UiTreeWalk::CancelCurrentTask()
{
    MSG msg;
    PeekMessage(&msg, NULL, 0, 0, PM_NOREMOVE);
    if ((msg.message >= msgThreadTest && msg.message < msgThreadLast) || (msg.message == WM_QUIT))
    {
        InvalidateTargetRect();
        return TRUE;
    }

    POINT pt;
    GetPhysicalCursorPos(&pt);
    if (abs(pt.x - m_pt.x) > MinDist || abs(pt.y - m_pt.y) > MinDist)
    {
        InvalidateTargetRect();
#ifndef _DEBUG
        return TRUE;
#else 
        return FALSE;
#endif
    }

    return FALSE;
}

long UiTreeWalk::UiTreeWalkerThreadProc(void* pVoidData)
{
    REQUIRE_SUCCESS_HR(CoInitializeEx(NULL, COINIT_MULTITHREADED));

    UiTreeWalk* pUiTreeWalk = new UiTreeWalk;

    HRESULT hr = pUiTreeWalk->InitAutomation();
    if (FAILED(hr))
    {
        CoUninitialize();
        REQUIRE_SUCCESS_HR(hr);
    }

    BOOL bRet;
    MSG msg;

    // Make sure the message queue is created
    PeekMessage(&msg, NULL, WM_USER, WM_USER, PM_NOREMOVE);
    SetEvent(UiTreeWalk::s_hEventPathReady);

    while ((bRet = GetMessage(&msg, NULL, 0, 0)) != 0)
    {
        if (bRet == -1)
        {
            break;
        }

        switch (msg.message)
        {

        case WM_TIMER:
        {
            pUiTreeWalk->DrawHighlight();
            break;
        }
        case msgGetXPath:
        {
            pUiTreeWalk->InvalidateTargetRect();
            pUiTreeWalk->PathFromRootToTarget(HIWORD(msg.lParam), LOWORD(msg.lParam));
            SetEvent(UiTreeWalk::s_hEventPathReady);
            break;
        }
        case msgHighlight:
        {
            pUiTreeWalk->InvalidateTargetRect();
            pUiTreeWalk->FindCachedUI();
            SetEvent(UiTreeWalk::s_hEventPathReady);
            break;
        }
        case msgThreadTest:
        {
            SetEvent(UiTreeWalk::s_hEventPathReady);
            break;
        }
        default:
        {
            TranslateMessage(&msg);
            DispatchMessage(&msg);
            break;
        }
        }
    }

    pUiTreeWalk->InvalidateTargetRect();
    pUiTreeWalk->UnInitAutomation();
    delete pUiTreeWalk;

    CoUninitialize();
    return 0;
}

void UiTreeWalk::FindCachedUI()
{
    CComBSTR runtimeId(s_chBuffer);

    auto it = cachedAutoElements.find(runtimeId);
    if (it != cachedAutoElements.end())
    {
        auto cachedUi = it->second;

        if (SUCCEEDED(cachedUi->get_CurrentBoundingRectangle(&s_rectTargetElement)))
        {
            DrawYellowHighlightRect(m_hdcDeskTop, s_rectTargetElement);
            Sleep(500);
            DrawYellowHighlightRect(m_hdcDeskTop, s_rectTargetElement);
        }
    }
}

long UiTreeWalk::HighlightCachedUI(_In_ LPWSTR lpRumtimeId, _Out_ RECT *pRect)
{
    swprintf_s(s_chBuffer, BUFFERSIZE, L"%s", lpRumtimeId);
    ResetEvent(UiTreeWalk::s_hEventPathReady);

    PostThreadMessage(UiTreeWalk::s_dwUiTreeWalkerThread, msgHighlight, 0, 0);

    if (WAIT_OBJECT_0 == WaitForSingleObject(UiTreeWalk::s_hEventPathReady, dwWaitUiWalk))
    {
        memcpy(pRect, &s_rectTargetElement, sizeof(RECT));
    }
    else
    {
        memset(pRect, 0, sizeof(RECT));
    }

    return 0;
}

HRESULT UiTreeWalk::GetParentUiNode(long left, long top, IUIAutomationTreeWalker *pTreeWalker, IUIAutomationElement* pCurUia, std::wstring& result)
{
    if (CancelCurrentTask())
    {
        return S_FALSE;
    }

    int nPos = -1;
    AppendUiAttributes(left, top, pCurUia, nPos, result);

    CComPtr<IUIAutomationElement> spParent;
    if (FAILED(pTreeWalker->GetParentElement(pCurUia, &spParent.p)))
    {
        // Edge workaround
        UIA_HWND uiaHwnd;
        if (SUCCEEDED(pCurUia->get_CurrentNativeWindowHandle(&uiaHwnd)))
        {
            HWND hwnd = GetAncestor(reinterpret_cast<HWND>(uiaHwnd), GA_PARENT);
            if (hwnd != nullptr)
            {
                REQUIRE_SUCCESS_HR(m_spAutomation->ElementFromHandle(hwnd, &spParent.p));
            }
        }
    }

    if (spParent != nullptr)
    {
        return GetParentUiNode(left, top, pTreeWalker, spParent, result);
    }
    else
    {
        return S_OK;
    }
}

HRESULT UiTreeWalk::PathFromTargetToRoot(long left, long top)
{
    m_pt = { left,top };
    CComPtr<IUIAutomationElement> spUia;

    REQUIRE_SUCCESS_HR(m_spAutomation->ElementFromPoint(m_pt, &spUia.p));

    REQUIRE_SUCCESS_HR(spUia->get_CurrentBoundingRectangle(&s_rectTargetElement));

    std::wstring uiPath;
    HRESULT hr = GetParentUiNode(left, top, m_spTreeWalker, spUia, uiPath);
    if (hr == S_OK)
    {
        swprintf_s(s_chBuffer, BUFFERSIZE, L"%s", uiPath.c_str());
        DrawHighlight();

        m_nHighlightTimer = SetTimer(NULL, 0, 500, NULL);
    }

    return hr;
}
