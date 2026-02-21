import { useState } from "react";
import { Difficulty, Status } from "../../Models/races.type.ts";
import CreateRace from "../../components/races/CreateRace.tsx";
import EditRace from "../../components/races/EditRace.tsx";
import ConfirmModal from "../../components/common/ConfirmModal.tsx";
import CountRegistrations from "../../components/common/CountRegistrations.tsx";
import { useOrganiserData } from "../../hooks/useOrganiserData.ts";
import { registrationService } from "../../services/registrationService.ts";
import obstacleService from "../../services/obstacleService.ts";

export function OrganiserDashboard() {
    const {
        races, obstacles, pendingRegistrations, loading, error,
        refreshData, fetchRegistrationsForRace, parseApiError
    } = useOrganiserData();

    const [activeTab, setActiveTab] = useState<'races' | 'obstacles' | 'registrations'>('races');
    const [selectedRaceFilter, setSelectedRaceFilter] = useState<number>(0);
    const [modalError, setModalError] = useState<string | null>(null);

    const [raceModal, setRaceModal] = useState<{
        isOpen: boolean; mode: 'create' | 'edit' | null; id: number | null;
    }>({ isOpen: false, mode: null, id: null });

    const [confirmConfig, setConfirmConfig] = useState<{
        isOpen: boolean; type: 'confirm' | 'cancel' | 'delete' | null; id: number | null;
    }>({ isOpen: false, type: null, id: null });

    const handleRaceSelect = (raceId: number) => {
        setSelectedRaceFilter(raceId);
        void fetchRegistrationsForRace(raceId);
    };

    const handleConfirmation = async () => {
        if (!confirmConfig.id || !confirmConfig.type) return;
        try {
            setModalError(null);
            if (confirmConfig.type === 'confirm') await registrationService.confirmUserRegistration(confirmConfig.id);
            if (confirmConfig.type === 'cancel') await registrationService.cancelUserRegistration(confirmConfig.id);
            if (confirmConfig.type === 'delete') await obstacleService.removeObstacle(confirmConfig.id);

            setConfirmConfig(prev => ({ ...prev, isOpen: false }));
            void refreshData();
        } catch (err) {
            setModalError(parseApiError(err));
        }
    };

    if (loading && races.length === 0) {
        return (
            <div className="min-h-screen bg-dark flex items-center justify-center">
                <div className="flex flex-col items-center gap-4">
                    <div className="w-12 h-12 border-2 border-accent border-t-transparent animate-spin" />
                    <p className="text-[10px] font-black uppercase tracking-[0.5em] text-accent animate-pulse italic">Synchronizing_Data...</p>
                </div>
            </div>
        );
    }

    if (error) {
        return (
            <div className="min-h-screen bg-dark flex items-center justify-center p-6">
                <div className="max-w-md w-full bg-red-500/10 border border-red-500/50 p-8 backdrop-blur-xl">
                    <h3 className="text-[10px] font-black uppercase tracking-widest text-red-500 mb-2">System_Failure</h3>
                    <p className="text-xs font-bold text-white/80 uppercase">{error}</p>
                    <button
                        onClick={() => window.location.reload()}
                        className="mt-4 text-[10px] font-black uppercase text-white hover:text-accent cursor-pointer"
                    >
                        [ Reboot ]
                    </button>
                </div>
            </div>
        );
    }

    return (
        <div className="min-h-screen bg-dark text-light p-4 md:p-6 lg:p-12">
            <div className="h-20 md:h-28" />
            <div className="flex flex-col md:flex-row justify-between items-start md:items-end gap-6 mb-10">
                <div className="space-y-1">
                    <h1 className="text-3xl md:text-5xl font-black uppercase italic text-white">
                        Organiser <span className="text-accent">Dashboard</span>
                    </h1>
                    <p className="text-[9px] uppercase tracking-[0.3em] text-white/30 font-bold italic">Unit Ops // {activeTab}</p>
                </div>
                <button
                    onClick={() => setRaceModal({ isOpen: true, mode: 'create', id: null })}
                    className="bg-accent text-dark px-8 py-3 font-black uppercase italic text-xs hover:scale-105 transition-all shadow-[0_0_25px_rgba(166,124,82,0.4)] cursor-pointer"
                >
                    + New Race
                </button>
            </div>

            <div className="flex gap-6 mb-12 border-b border-white/5 overflow-x-auto no-scrollbar">
                {['races', 'obstacles', 'registrations'].map((tab) => (
                    <button
                        key={tab}
                        onClick={() => setActiveTab(tab as any)}
                        className={`pb-4 text-[10px] font-black uppercase tracking-[0.2em] transition-all cursor-pointer ${
                            activeTab === tab ? 'text-accent border-b-2 border-accent' : 'text-white/20 hover:text-white/60'
                        }`}
                    >
                        {tab === 'registrations' ? 'Personnel Clearances' : `My ${tab}`}
                    </button>
                ))}
            </div>

            {activeTab === 'registrations' && (
                <div className="mb-8 flex items-center gap-4 bg-white/3 p-4 border border-white/5">
                    <label className="text-[10px] font-black uppercase tracking-widest text-accent italic">Select Race:</label>
                    <select
                        value={selectedRaceFilter}
                        onChange={(e) => handleRaceSelect(Number(e.target.value))}
                        className="bg-dark border border-white/10 text-white text-[10px] font-black uppercase px-4 py-2 outline-none focus:border-accent cursor-pointer"
                    >
                        <option value={0}>-- ALL ACTIVE MISSIONS --</option>
                        {races.map(race => <option key={race.id} value={race.id}>{race.name}</option>)}
                    </select>
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
                    <tbody className="divide-y divide-white/5 uppercase italic font-black">
                    {activeTab === 'races' && races.length > 0 && races.map(race => (
                        <tr key={race.id} className="hover:bg-white/3 transition-all group border-l-2 border-l-transparent hover:border-l-accent">
                            <td className="p-6">
                                <div className="text-white text-lg font-black">{race.name}</div>
                                <div className="text-[9px] text-white/20 font-bold uppercase">{race.location} // {race.distance}KM</div>
                            </td>
                            <td className="p-6 text-center">
                                <div className="text-white text-lg"><CountRegistrations raceId={race.id}/> / {race.maxParticipants}</div>
                                <div className="text-[9px] text-white/20 uppercase">Participants</div>
                            </td>
                            <td className="p-6 text-center">
                                <span className="text-[10px] px-3 py-1 border border-accent/20 bg-accent/10">{Status[race.status]}</span>
                            </td>
                            <td className="p-6 text-right">
                                <button
                                    onClick={() => setRaceModal({ isOpen: true, mode: 'edit', id: race.id })}
                                    className="text-accent text-[10px] hover:brightness-125 cursor-pointer"
                                >
                                    [ MODIFY ]
                                </button>
                            </td>
                        </tr>
                    ))}

                    {activeTab === 'obstacles' && obstacles.length > 0 && obstacles.map(obs => (
                        <tr key={obs.id} className="hover:bg-white/3 transition-all border-l-2 border-l-transparent hover:border-l-white">
                            <td className="p-6">
                                <div className="text-white text-lg font-black">{obs.name}</div>
                                <div className="text-[9px] text-white/30 truncate max-w-xs not-italic font-bold lowercase first-letter:uppercase">
                                    {obs.description || "No description provided"}
                                </div>
                            </td>
                            <td className="p-6 text-center">
                                <span className={`px-4 py-2 border text-[11px] ${
                                    obs.difficulty === 2 ? 'text-red-500 border-red-500/30 bg-red-500/10' :
                                        obs.difficulty === 1 ? 'text-orange-500 border-orange-500/30 bg-orange-500/10' :
                                            'text-green-500 border-green-500/30 bg-green-500/10'
                                }`}>
                                    {Difficulty[obs.difficulty]}
                                </span>
                            </td>
                            <td className="p-6 text-center">
                                <div className="text-[9px] text-white/20">Created</div>
                            </td>
                            <td className="p-6 text-right">
                                <button
                                    onClick={() => {setConfirmConfig({ isOpen: true, type: 'delete', id: obs.id });setModalError(null);}}
                                    className="text-white/40 text-[10px] hover:text-red-500 transition-colors cursor-pointer"
                                >
                                    [ DELETE ]
                                </button>
                            </td>
                        </tr>
                    ))}

                    {activeTab === 'registrations' && pendingRegistrations.length > 0 && pendingRegistrations.map(reg => (
                        <tr key={reg.id} className="hover:bg-white/3 transition-all border-l-2 border-l-transparent hover:border-l-accent">
                            <td className="p-6">
                                <div className="text-white text-lg font-black">{reg.participant.name} {reg.participant.surname}</div>
                                <div className="text-accent text-[10px]">BIB: #{reg.bibNumber} // {reg.race?.name}</div>
                            </td>
                            <td className="p-6 text-center">
                                <div className="text-[16px] text-white/30 not-italic font-bold">
                                    {reg.user.phoneNumber || 'N/A'}
                                </div>
                            </td>
                            <td className="p-6 text-center">
                                <span className="text-[16px] bg-orange-500/10 text-orange-500 px-3 py-1 border border-orange-500/20 animate-pulse">PENDING</span>
                            </td>
                            <td className="p-6 text-right space-x-3">
                                <button
                                    onClick={() => {setConfirmConfig({ isOpen: true, type: 'confirm', id: reg.id })}}
                                    className="text-green-500 border border-green-500/20 px-4 py-2 hover:bg-green-500 hover:text-white transition-all cursor-pointer"
                                >
                                    [ CONFIRM ]
                                </button>
                                <button
                                    onClick={() => setConfirmConfig({ isOpen: true, type: 'cancel', id: reg.id })}
                                    className="text-red-500 border border-red-500/20 px-4 py-2 hover:bg-red-500 hover:text-white transition-all cursor-pointer"
                                >
                                    [ CANCEL ]
                                </button>
                            </td>
                        </tr>
                    ))}
                    </tbody>
                </table>

                {activeTab === 'races' && races.length === 0 && (
                    <div className="p-20 text-center">
                        <div className="text-white/5 text-6xl mb-4">🏁</div>
                        <p className="text-white/10 text-[10px] tracking-[0.5em] font-black uppercase italic mb-6">NO_RACES_CREATED</p>
                        <button
                            onClick={() => setRaceModal({ isOpen: true, mode: 'create', id: null })}
                            className="bg-accent text-dark px-8 py-3 font-black uppercase italic text-xs hover:scale-105 transition-all cursor-pointer"
                        >
                            + Create Your First Race
                        </button>
                    </div>
                )}

                {activeTab === 'obstacles' && obstacles.length === 0 && (
                    <div className="p-20 text-center">
                        <div className="text-white/5 text-6xl mb-4">🚧</div>
                        <p className="text-white/10 text-[10px] tracking-[0.5em] font-black uppercase italic">NO_OBSTACLES_FOUND</p>
                        <p className="text-white/5 text-[8px] mt-2 not-italic font-bold">Create a race first to add obstacles</p>
                    </div>
                )}

                {activeTab === 'registrations' && pendingRegistrations.length === 0 && (
                    <div className="p-20 text-center">
                        <div className="text-white/5 text-6xl mb-4">📋</div>
                        <p className="text-white/10 text-[10px] tracking-[0.5em] font-black uppercase italic">NO_PENDING_REGISTRATIONS</p>
                        <p className="text-white/5 text-[8px] mt-2 not-italic font-bold">
                            {selectedRaceFilter === 0 ? 'All registrations have been processed' : 'No pending registrations for selected race'}
                        </p>
                    </div>
                )}
            </div>
            {raceModal.isOpen && (
                raceModal.mode === 'create' ?
                    <CreateRace isOpen={true} onClose={() => setRaceModal({ ...raceModal, isOpen: false })} onSuccess={refreshData} /> :
                    <EditRace id={raceModal.id || 0} isOpen={true} onClose={() => setRaceModal({ ...raceModal, isOpen: false })} onSuccess={refreshData} />
            )}
            <ConfirmModal
                isOpen={confirmConfig.isOpen}
                onClose={() => {setConfirmConfig(p => ({ ...p, isOpen: false })); setModalError(null)}}
                onConfirm={handleConfirmation}
                title={confirmConfig.type === 'confirm' ? "AUTHORIZE" : "TERMINATE"}
                variant={confirmConfig.type === 'confirm' ? "success" : "danger"}
                message="CONFIRM ACTION FOR SELECTED UNIT?"
                error={modalError}
            />
        </div>
    );
}
export default OrganiserDashboard;