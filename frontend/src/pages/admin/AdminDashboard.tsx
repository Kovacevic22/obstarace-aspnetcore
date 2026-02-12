import CreateRace from "../../components/races/CreateRace.tsx";
import EditRace from "../../components/races/EditRace.tsx";
import ConfirmModal from "../../components/common/ConfirmModal.tsx";
import { useAdminData } from "../../hooks/useAdminData.ts";
import { useAuth } from "../../hooks/useAuth.ts";
import {useState} from "react";
import {StatCard} from "../../components/common/StatCard.tsx";
import {Status} from "../../Models/races.type.ts";
import {Role} from "../../Models/users.type.ts";

export function AdminDashboard() {
    const { refreshUser } = useAuth();
    const {
        races, users, raceStats, userStats, pendingOrganisers,
        loading, error, fetchDashboardData, executeAction
    } = useAdminData();

    const [activeTab, setActiveTab] = useState<'races' | 'users' | 'applications'>('races');
    const [isCreateRaceOpen, setIsCreateRaceOpen] = useState(false);
    const [isEditRaceOpen, setIsEditRaceOpen] = useState(false);
    const [selectedRaceId, setSelectedRaceId] = useState<number | null>(null);
    const [confirmConfig, setConfirmConfig] = useState<{
        isOpen: boolean; type: 'approve' | 'reject' | 'ban' | 'unban' | null;
        userId: number | null; userName: string;
    }>({ isOpen: false, type: null, userId: null, userName: "" });

    const handleAction = async () => {
        if (!confirmConfig.userId || !confirmConfig.type) return;
        const success = await executeAction(confirmConfig.type, confirmConfig.userId);
        if (success) {
            setConfirmConfig(prev => ({ ...prev, isOpen: false }));

            if (confirmConfig.type === 'ban' || confirmConfig.type === 'unban') {
                await refreshUser();
            }
        }
    };

    const openEditModal = (id: number) => {
        setSelectedRaceId(id);
        setIsEditRaceOpen(true);
    };

    if (loading && races.length === 0) return (
        <div className="min-h-screen bg-dark flex items-center justify-center">
            <div className="text-accent font-black uppercase tracking-[0.5em] animate-pulse italic">Loading_System...</div>
        </div>
    );

    return (
        <div className="min-h-screen bg-dark text-light p-4 md:p-6 lg:p-12">
            <div className="h-20 md:h-28" />
            <div className="flex flex-col md:flex-row justify-between items-start md:items-end gap-6 mb-10">
                <div className="space-y-1">
                    <h1 className="text-3xl md:text-5xl font-black uppercase italic text-white">Command Center</h1>
                    <p className="text-[9px] uppercase tracking-[0.3em] text-white/30 font-bold italic">Sector: {activeTab}</p>
                </div>
                {activeTab === 'races' && (
                    <button
                        onClick={() => setIsCreateRaceOpen(true)}
                        className="bg-accent text-dark px-8 py-3 font-black uppercase italic text-xs hover:scale-105 transition-all cursor-pointer"
                    >
                        + Create New Race
                    </button>
                )}
            </div>

            <div className="flex gap-6 mb-12 border-b border-white/5 overflow-x-auto no-scrollbar">
                {['races', 'users', 'applications'].map(tab => (
                    <button
                        key={tab}
                        onClick={() => setActiveTab(tab as any)}
                        className={`pb-4 text-[10px] font-black uppercase tracking-[0.2em] transition-all cursor-pointer ${activeTab === tab ? 'text-accent border-b-2 border-accent' : 'text-white/20 hover:text-white/50'}`}
                    >
                        {tab.replace('applications', 'Pending Applications')}
                    </button>
                ))}
            </div>

            {activeTab !== 'applications' && (
                <div className="grid grid-cols-2 md:grid-cols-4 gap-6 mb-12 font-black italic">
                    {activeTab === 'races' ? (
                        <>
                            <StatCard label="Total Races" value={raceStats?.totalRaces} />
                            <StatCard label="Total KM" value={raceStats?.totalKilometers} color="text-accent" />
                            <StatCard label="Archived" value={raceStats?.archivedCount} opacity="opacity-40" />
                        </>
                    ) : (
                        <>
                            <StatCard label="Total Users" value={userStats?.totalUsers} />
                            <StatCard label="Banned Users" value={userStats?.bannedUsers} color="text-secondary" />
                        </>
                    )}
                </div>
            )}

            <div className="bg-white/2 border border-white/10 overflow-x-auto shadow-2xl">
                <table className="w-full text-left border-collapse">
                    <thead className="text-[10px] uppercase tracking-[0.2em] text-white/20 border-b border-white/10 bg-white/1">
                    <tr>
                        <th className="p-6 italic">Designation</th>
                        <th className="p-6 text-center italic">Details</th>
                        <th className="p-6 text-center italic">Status</th>
                        <th className="p-6 text-right italic">Operations</th>
                    </tr>
                    </thead>
                    <tbody className="divide-y divide-white/5 font-black uppercase italic">
                    {activeTab === 'races' && races.map(race => (
                        <tr key={race.id} className="hover:bg-white/3 transition-all group border-l-2 border-l-transparent hover:border-l-accent">
                            <td className="p-6">
                                <div className="text-white text-lg">{race.name}</div>
                                <div className="text-[9px] text-white/20 font-bold uppercase">{race.location} // {race.distance}KM</div>
                            </td>
                            <td className="p-6 text-center">
                                <div className="text-white text-base font-black italic">{race.maxParticipants} <span className="text-[9px] text-white/20">MAX</span></div>
                                <div className="text-[9px] text-white/20 uppercase">+{race.elevationGain}m Elevation</div>
                            </td>
                            <td className="p-6 text-center">
                                <span className="text-[10px] px-3 py-1 border border-accent/20 bg-accent/10 font-black uppercase italic">{Status[race.status]}</span>
                            </td>
                            <td className="p-6 text-right">
                                <button
                                    onClick={() => openEditModal(race.id)}
                                    className="text-accent text-[10px] hover:brightness-125 font-black uppercase italic cursor-pointer"
                                >
                                    [ MODIFY ]
                                </button>
                            </td>
                        </tr>
                    ))}

                    {activeTab === 'users' && users?.map(user => (
                        <tr key={user.id} className="hover:bg-white/3 transition-all group border-l-2 border-l-transparent hover:border-l-white">
                            <td className="p-6">
                                <div className="text-white text-lg">
                                    {user.participant ? `${user.participant.name} ${user.participant.surname}` : user.email}
                                </div>
                                <div className="text-[9px] text-white/30 font-bold lowercase italic">{user.email}</div>
                            </td>
                            <td className="p-6 text-center">
                                <div className="text-white/60 text-xs font-bold uppercase">
                                    {Role[user.role]}
                                </div>
                            </td>
                            <td className="p-6 text-center">
                                <span className={`text-[10px] px-3 py-1 border font-black uppercase italic ${
                                    user.banned
                                        ? 'border-red-500/30 text-red-500 bg-red-500/10'
                                        : 'border-green-500/30 text-green-500 bg-green-500/10'
                                }`}>
                                    {user.banned ? 'BANNED' : 'ACTIVE'}
                                </span>
                            </td>
                            <td className="p-6 text-right">
                                {user.role === 1 ? (
                                    <span className="text-white/10 text-[10px] font-black uppercase italic cursor-not-allowed">
                                        [ ADMIN ]
                                    </span>
                                ) : user.banned ? (
                                    <button
                                        onClick={() => setConfirmConfig({ isOpen: true, type: 'unban', userId: user.id, userName: user.email })}
                                        className="text-green-500 text-[10px] hover:brightness-125 font-black uppercase italic cursor-pointer"
                                    >
                                        [ UNBAN ]
                                    </button>
                                ) : (
                                    <button
                                        onClick={() => setConfirmConfig({ isOpen: true, type: 'ban', userId: user.id, userName: user.email })}
                                        className="text-red-500 text-[10px] hover:brightness-125 font-black uppercase italic cursor-pointer"
                                    >
                                        [ BAN ]
                                    </button>
                                )}
                            </td>
                        </tr>
                    ))}
                    </tbody>
                </table>
            </div>

            {activeTab === 'applications' && (
                <div className="space-y-6">
                    {pendingOrganisers.length > 0 ? (
                        pendingOrganisers.map(org => (
                            <div key={org.userId} className="bg-white/3 border border-white/10 p-8 flex flex-col md:flex-row justify-between items-start md:items-center gap-6 hover:bg-white/5 hover:border-accent/40 transition-all shadow-xl">
                                <div className="space-y-3 flex-1">
                                    <div className="flex items-center gap-3">
                                        <div className="text-accent text-[9px] font-black uppercase tracking-widest border border-accent/20 px-3 py-1 inline-block bg-accent/5">
                                            Application ID: #{org.userId}
                                        </div>
                                        <div className="text-[9px] text-white/20 font-mono uppercase">
                                            {org.organisationName}
                                        </div>
                                    </div>
                                    <h3 className="text-2xl font-black text-white uppercase italic">
                                        {org.userName} {org.userSurname}
                                    </h3>
                                    <div className="bg-white/5 border-l-2 border-accent/30 pl-4 py-2">
                                        <p className="text-xs text-white/60 italic leading-relaxed">
                                            "{org.description}"
                                        </p>
                                    </div>
                                </div>
                                <div className="flex flex-row md:flex-col gap-3 w-full md:w-auto">
                                    <button
                                        onClick={() => setConfirmConfig({ isOpen: true, type: 'approve', userId: org.userId, userName: org.organisationName })}
                                        className="flex-1 md:flex-none bg-green-600/20 text-green-500 border border-green-500/30 px-6 py-3 text-[10px] font-black uppercase italic hover:bg-green-600/30 transition-all cursor-pointer"
                                    >
                                        [ Approve ]
                                    </button>
                                    <button
                                        onClick={() => setConfirmConfig({ isOpen: true, type: 'reject', userId: org.userId, userName: org.organisationName })}
                                        className="flex-1 md:flex-none bg-red-600/20 text-red-500 border border-red-500/30 px-6 py-3 text-[10px] font-black uppercase italic hover:bg-red-600/30 transition-all cursor-pointer"
                                    >
                                        [ Deny ]
                                    </button>
                                </div>
                            </div>
                        ))
                    ) : (
                        <div className="p-20 text-center border-2 border-dashed border-white/5 bg-white/1">
                            <p className="text-white/10 text-[10px] tracking-[0.5em] font-black uppercase italic">NO_PENDING_APPLICATIONS</p>
                        </div>
                    )}
                </div>
            )}

            <CreateRace isOpen={isCreateRaceOpen} onClose={() => setIsCreateRaceOpen(false)} onSuccess={fetchDashboardData} />
            <EditRace isOpen={isEditRaceOpen} onClose={() => { setIsEditRaceOpen(false); setSelectedRaceId(null); }} id={selectedRaceId || 0} onSuccess={fetchDashboardData} />

            <ConfirmModal
                isOpen={confirmConfig.isOpen}
                onClose={() => setConfirmConfig(prev => ({ ...prev, isOpen: false }))}
                onConfirm={handleAction}
                title={confirmConfig.type === 'approve' ? "AUTHORIZATION" : "WARNING"}
                variant={confirmConfig.type === 'approve' ? "success" : "danger"}
                message={`CONFIRM ${confirmConfig.type?.toUpperCase()} FOR: ${confirmConfig.userName.toUpperCase()}?`}
                error={error}
            />
        </div>
    );
}

export default AdminDashboard;