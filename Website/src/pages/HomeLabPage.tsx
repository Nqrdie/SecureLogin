import { useEffect, useState } from 'react'
import { CircularProgressbar, buildStyles } from 'react-circular-progressbar'
import 'react-circular-progressbar/dist/styles.css'

type Container = {
  id: number
  name: string
  status: 'running' | 'stopped'
}

export default function HomeLabPage() {
  const [cpu, setCpu] = useState(35)
  const [ram, setRam] = useState(60)
  const [disk, setDisk] = useState(72)

  const [containers, setContainers] = useState<Container[]>([
    { id: 1, name: 'Plex', status: 'running' },
    { id: 2, name: 'Nextcloud', status: 'running' },
    { id: 3, name: 'Scaletail', status: 'stopped' },
    { id: 4, name: 'Immich', status: 'running' },
    { id: 5, name: 'Vaultwarden', status: 'running'},
    { id: 6, name: 'Portainer', status: 'running'},
  ])

  useEffect(() => {
    const interval = setInterval(() => {
      setCpu(Math.floor(Math.random() * 80) + 10)
      setRam(Math.floor(Math.random() * 80) + 10)
      setDisk(Math.floor(Math.random() * 40) + 40)
    }, 2000)

    return () => clearInterval(interval)
  }, [])

  function toggleContainer(id: number) {
    setContainers(prev =>
      prev.map(c =>
        c.id === id
          ? {
              ...c,
              status: c.status === 'running' ? 'stopped' : 'running',
            }
          : c
      )
    )
  }

  function logout() {
    localStorage.removeItem("token")
    window.location.href = '/login'
  }

  return (
    <div className="dashboard">
        <button onClick={logout} className="logout-button">
        Logout
        </button>
      <h1 className="title">HomeLab Dashboard</h1>

      {/* GAUGES */}
      <div className="gauge-grid">
        
        <div className="gauge-card">
          <h3>CPU</h3>
          <CircularProgressbar
            value={cpu}
            text={`${cpu}%`}
            styles={buildStyles({
              pathColor: '#22c55e',
              textColor: '#fff',
              trailColor: '#374151',
            })}
          />
        </div>

        <div className="gauge-card">
          <h3>RAM</h3>
          <CircularProgressbar
            value={ram}
            text={`${ram}%`}
            styles={buildStyles({
              pathColor: '#3b82f6',
              textColor: '#fff',
              trailColor: '#374151',
            })}
          />
        </div>

        <div className="gauge-card">
          <h3>DISK</h3>
          <CircularProgressbar
            value={disk}
            text={`${disk}%`}
            styles={buildStyles({
              pathColor: '#f59e0b',
              textColor: '#fff',
              trailColor: '#374151',
            })}
          />
        </div>
      </div>

      {/* CONTAINERS */}
      <h2 className="section">Containers</h2>

      <div className="container-list">
        {containers.map(c => (
          <div key={c.id} className="container-card">
            <div>
              <strong>{c.name}</strong>
              <p className={c.status}>{c.status}</p>
            </div>

            <button onClick={() => toggleContainer(c.id)}>
              {c.status === 'running' ? 'Stop' : 'Start'}
            </button>
          </div>
        ))}
      </div>
    </div>
  )
}