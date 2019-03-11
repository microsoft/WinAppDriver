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

#pragma once
#include "stdafx.h"

const long BUFFERSIZE = 4096 * 4; // must be same as BUFFERSIZE defined in Win32API.cs

enum UiTreeWalkMessage
{
    msgThreadTest = WM_USER + 1,
    msgGetXPath,
    msgHighlight,
    //Insert before here
    msgThreadLast
};

class UiTreeWalk
{
public:
    static void Init();
    static void UnInit();
    static long UiTreeWalkerThreadProc(void*);
    static long GetUiXPath(_In_ long left, _In_ long top, _Out_writes_to_(nMaxCount, return) LPWSTR lpUiPath, _In_ long nMaxCount);
    static long HighlightCachedUI(_In_ LPWSTR lpRumtimeId, _Out_ RECT *pRect);

    UiTreeWalk() noexcept;
    virtual ~UiTreeWalk();

private:
    HRESULT InitAutomation();
    void UnInitAutomation();
    HRESULT PathFromRootToTarget(long left, long top);
    HRESULT GetChildUiNode(long left, long top, IUIAutomationElement* pCur, IUIAutomationElement* pFromPoint, std::wstring& uiPath);
    HRESULT ElementContainingPoint(long left, long top, IUIAutomationElement*pCur, IUIAutomationElement** ppElem);
    BOOL CancelCurrentTask();
    HRESULT AppendUiAttributes(long left, long top, IUIAutomationElement* pCurUia, long nPos, std::wstring& wstr);
    void InvalidateTargetRect();
    void DrawHighlight();
    void FindCachedUI();

    HRESULT PathFromTargetToRoot(long left, long top);
    HRESULT GetParentUiNode(long left, long top, IUIAutomationTreeWalker *pTreeWalker, IUIAutomationElement* pCurUia, std::wstring& result);

    static HANDLE s_hEventPathReady;
    static HANDLE s_hUiTreeWalkerThread;
    static DWORD s_dwUiTreeWalkerThread;
    static WCHAR s_chBuffer[BUFFERSIZE];
    static RECT s_rectTargetElement;

    CComPtr<IUIAutomation> m_spAutomation;
    CComPtr<IUIAutomationCondition> m_spFindAllCondition;
    CComPtr<IUIAutomationTreeWalker> m_spTreeWalker;
    std::map<CComBSTR, CComPtr<IUIAutomationElement>> cachedAutoElements;

    HDC m_hdcDeskTop;
    HWND m_hWndTarget;
    POINT m_pt;
    UINT_PTR m_nHighlightTimer;
};
