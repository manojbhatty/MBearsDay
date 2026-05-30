import { useState, useEffect } from 'react'
import type { ReportIndexEntry } from '../types'

interface State {
  entries: ReportIndexEntry[]
  loading: boolean
}

export function useReportIndex(): State {
  const [state, setState] = useState<State>({ entries: [], loading: true })

  useEffect(() => {
    fetch('/reports/reports-index.json')
      .then(r => (r.ok ? (r.json() as Promise<ReportIndexEntry[]>) : Promise.reject()))
      .then(entries => setState({ entries, loading: false }))
      .catch(() => setState({ entries: [], loading: false }))
  }, [])

  return state
}
