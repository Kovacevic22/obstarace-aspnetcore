import {useEffect, useState} from "react";
import ConfirmModal from "../common/ConfirmModal.tsx";
import type {UpdateRaceDto} from "../../Models/races.type.ts";
import raceService from "../../services/raceService.ts";
import obstacleService from "../../services/obstacleService.ts";
import {type CreateObstacleDto, Difficulty, type ObstacleDto} from "../../Models/obstacles.type.ts";
import * as React from "react";

export function EditRace({ isOpen, onClose, id, onSuccess }: { isOpen: boolean, onClose: () => void,id:number, onSuccess: () => void }) {

    const [isDeleteModalOpen, setIsDeleteModalOpen] = useState(false);
    const [isObstacleDeleteModalOpen, setIsObstacleDeleteModalOpen] = useState(false);
    const [isLoading, setIsLoading] = useState(true);
    const [itemToDelete, setItemToDelete] = useState<number | null>(null);
    const [allObstacles, setAllObstacles] = useState<ObstacleDto[]>([]);
    const [error, setError] = useState<string|null>("");
    const [newObstacle, setNewObstacle] = useState<CreateObstacleDto>({
        name: '',
        description: '',
        difficulty: 0
    });
    const [raceForm, setRaceForm] = useState<UpdateRaceDto>({
        name: '',
        slug: '',
        date: '',
        registrationDeadLine: '',
        description: '',
        location: '',
        distance: 0,
        difficulty: 0,
        status: 0,
        imageUrl: '',
        elevationGain: 0,
        maxParticipants: 0,
        obstacleIds: []
    });
     const isFormValid =
        raceForm.name.trim() !== "" &&
        raceForm.location.trim() !== "" &&
        raceForm.description.trim() !== "" &&
        raceForm.distance > 0 &&
        raceForm.elevationGain > 0 &&
        raceForm.imageUrl.trim()!== "" &&
        raceForm.maxParticipants > 0;

    const isCreateObstacleValid =
        newObstacle.name.trim() !=="" &&
        newObstacle.description.trim() !=="";

    const handleObstacleToggle = (obstacleId: number) => {
        setRaceForm(prev => ({
            ...prev,
            obstacleIds: prev.obstacleIds.includes(obstacleId)
                ? prev.obstacleIds.filter(id => id !== obstacleId)
                : [...prev.obstacleIds, obstacleId]
        }));
    };

    const slugGenerator = (e: React.ChangeEvent<HTMLInputElement>) => {
        const name = e.target.value;
        const slug = name.toLowerCase().replace(/[^a-z0-9]+/g, '-').replace(/(^-|-$)/g, '');
        setRaceForm(prev => ({ ...prev, name, slug }));
    };
    const parseApiError = (error: any): string => {
        const data = error?.response?.data;

        if (data?.errors) {
            return Object.values(data.errors).flat().join(" | ");
        }

        return data?.title || data?.error || "Error";
    };
    const deleteObstacle = async ()=>{
        if(itemToDelete){
            try{
                await obstacleService.removeObstacle(itemToDelete);
                setAllObstacles(prev => prev.filter(o => o.id !== itemToDelete));
                setRaceForm(prev => ({...prev, obstacleIds: prev.obstacleIds.filter(oid => oid !== itemToDelete)}));
                setItemToDelete(null);
                setIsDeleteModalOpen(false);
                setError(null);
            }catch(e){
                console.error(e);
                setError(parseApiError(e));
            }
        }
    }

    const deleteRace = async () => {
        try{
            setIsLoading(true);
            await raceService.deleteRace(id);
            onClose();
            onSuccess();
        }catch(e){
            console.error(e);
            setError(parseApiError(e));
        }finally {
            setIsLoading(false);
        }
    }
    const updateRace = async ()=>{
        try{
            setIsLoading(true)
            await raceService.updateRace(id,raceForm);
            onClose();
            onSuccess();
        }catch(e){
            console.error(e);
            setError(parseApiError(e));
        }finally {
            setIsLoading(false)
        }
    }
    useEffect(() => {
        if (isOpen && id) {
            const fetchData = async () => {
                try {
                    setIsLoading(true);
                    const [raceData, systemObstacles] = await Promise.all([
                        raceService.getRaceById(id),
                        obstacleService.getObstacles()
                    ]);
                    if (raceData) {
                        setRaceForm({
                            name: raceData.name || '',
                            slug: raceData.slug || '',
                            date: raceData.date ? new Date(raceData.date).toISOString().slice(0, 16) : '',
                            registrationDeadLine: raceData.registrationDeadLine ? new Date(raceData.registrationDeadLine).toISOString().slice(0, 16) : '',
                            description: raceData.description || '',
                            location: raceData.location || '',
                            distance: raceData.distance || 0,
                            difficulty: raceData.difficulty || 0,
                            status: raceData.status || 0,
                            imageUrl: raceData.imageUrl || '',
                            elevationGain: raceData.elevationGain || 0,
                            maxParticipants: raceData.maxParticipants || 0,
                            obstacleIds: raceData.obstacleIds || []
                        });
                    }
                    setAllObstacles(systemObstacles);
                } catch (err) {
                    console.error(err);
                    setError(parseApiError(err))
                } finally {
                    setIsLoading(false);
                }
            }
            void fetchData();
        }
    }, [isOpen, id]);

    if (!isOpen) return null;

    return (
        <div className="fixed inset-0 z-150 flex items-center justify-center p-0 md:p-6 font-black uppercase italic text-white">
            <div className="absolute inset-0 bg-black/95 backdrop-blur-xl" onClick={onClose} />

            <div className="relative w-full max-w-6xl h-full md:h-auto md:max-h-[95vh] overflow-y-auto bg-[#1a2421] border border-white/5 shadow-2xl no-scrollbar text-white">

                <div className="sticky top-0 z-10 bg-[#1a2421]/90 backdrop-blur-md p-6 md:p-8 border-b border-white/10 flex justify-between items-center">
                    <div>
                        <h2 className="text-3xl md:text-4xl tracking-tighter text-accent italic leading-none">Modify Race</h2>
                        <p className="text-[10px] tracking-[0.3em] mt-2 not-italic font-medium uppercase text-xs text-white/50">System Override // Edit Existing Parameters</p>
                    </div>
                    <button onClick={onClose} className="text-white/20 hover:text-white text-[10px] border border-white/10 px-4 py-2 transition-all hover:bg-white/5 cursor-pointer uppercase">
                        [ DISCARD ]
                    </button>
                </div>

                <div className="p-6 md:p-8 space-y-12 font-black italic">
                    <section className="grid grid-cols-1 md:grid-cols-4 gap-x-6 gap-y-8">
                        <div className="md:col-span-3 space-y-2">
                            <label className="text-[10px] text-white/40 tracking-widest ml-1 uppercase">
                                Race Name {raceForm.name.trim() === "" && <span className="text-red-500 animate-pulse ml-2">(Required)</span>}
                            </label>
                            <input className="w-full bg-white/3 border border-white/10 p-4 outline-none focus:border-accent text-white italic font-black uppercase"
                                   value={raceForm.name}
                                   onChange={slugGenerator}
                            />
                        </div>
                        <div className="space-y-2">
                            <label className="text-[10px] tracking-widest ml-1 uppercase text-xs text-white/20">Slug (Read Only)</label>
                            <input readOnly className="w-full bg-white/1 border border-white/5 p-4 outline-none text-white/20 cursor-not-allowed uppercase"
                                   value={raceForm.slug}
                            />
                        </div>
                        <div className="md:col-span-2 space-y-2">
                            <label className="text-[10px] text-accent tracking-widest ml-1 uppercase text-xs">Race Start Date & Time</label>
                            <input type="datetime-local" style={{ colorScheme: 'dark' }} className="w-full bg-white/3 border border-white/10 p-5 outline-none focus:border-accent text-lg shadow-inner text-white font-black uppercase"
                                   value={raceForm.date}
                                   onChange={(e)=> setRaceForm({...raceForm, date: e.target.value})}
                            />
                        </div>
                        <div className="md:col-span-2 space-y-2">
                            <label className="text-[10px] text-white/40 tracking-widest ml-1 uppercase text-xs">
                                Location {raceForm.location.trim() === "" && <span className="text-red-500 animate-pulse ml-2">(Required)</span>}
                            </label>
                            <input className="w-full bg-white/3 border border-white/10 p-5 outline-none focus:border-accent text-lg text-white font-black italic uppercase"
                                   value={raceForm.location}
                                   onChange={(e)=> setRaceForm({...raceForm, location: e.target.value})}
                            />
                        </div>
                        <div className="space-y-2">
                            <label className="text-[10px] text-white/40 tracking-widest ml-1 uppercase text-xs">
                                Distance (KM) {raceForm.distance <= 0 && <span className="text-red-500 animate-pulse ml-2">(Empty)</span>}
                            </label>
                            <input type="number" step="0.1" className="w-full bg-white/3 border border-white/10 p-4 outline-none focus:border-accent text-lg font-black text-white"
                                   value={raceForm.distance}
                                   onChange={(e)=> setRaceForm({...raceForm, distance: Number(e.target.value)})}
                            />
                        </div>
                        <div className="space-y-2">
                            <label className="text-[10px] text-white/40 tracking-widest ml-1 uppercase text-xs">Difficulty</label>
                            <select
                                value={raceForm.difficulty}
                                onChange={(e) => setRaceForm({...raceForm, difficulty: Number(e.target.value)})}
                                className="w-full bg-white/3 border border-white/10 p-4 outline-none cursor-pointer focus:border-accent uppercase font-black italic text-white">
                                <option value="0">Easy</option>
                                <option value="1">Normal</option>
                                <option value="2">Hard</option>
                            </select>
                        </div>
                        <div className="space-y-2">
                            <label className="text-[10px] text-white/40 tracking-widest ml-1 uppercase text-xs">
                                Max Participants {raceForm.maxParticipants <= 0 && <span className="text-red-500 animate-pulse ml-2">(Empty)</span>}
                            </label>
                            <input type="number" className="w-full bg-white/3 border border-white/10 p-4 outline-none focus:border-accent text-lg font-black text-white"
                                   value={raceForm.maxParticipants}
                                   onChange={(e)=> setRaceForm({...raceForm, maxParticipants: Number(e.target.value)})}
                            />
                        </div>
                        <div className="space-y-2">
                            <label className="text-[10px] text-white/40 tracking-widest ml-1 uppercase text-xs">
                                Elevation Gain (M) {raceForm.elevationGain <= 0 && <span className="text-red-500 animate-pulse ml-2">(Empty)</span>}
                            </label>
                            <input type="number" className="w-full bg-white/3 border border-white/10 p-4 outline-none focus:border-accent text-lg font-black text-white"
                                   value={raceForm.elevationGain}
                                   onChange={(e)=>setRaceForm({...raceForm, elevationGain: Number(e.target.value)})}
                            />
                        </div>

                        <div className="md:col-span-2 space-y-2">
                            <label className="text-[10px] text-accent tracking-widest ml-1 uppercase text-xs">Registration Deadline</label>
                            <input type="datetime-local" style={{ colorScheme: 'dark' }} className="w-full bg-white/3 border border-white/10 p-5 outline-none focus:border-accent text-lg shadow-inner text-white font-black uppercase"
                                   value={raceForm.registrationDeadLine}
                                   onChange={(e)=> setRaceForm({...raceForm, registrationDeadLine: e.target.value})}
                            />
                        </div>
                        <div className="md:col-span-2 space-y-2">
                            <label className="text-[10px] text-white/40 tracking-widest ml-1 uppercase text-xs">
                                Banner Image URL {raceForm.imageUrl.trim() === "" && <span className="text-red-500 animate-pulse ml-2">(Required)</span>}
                            </label>
                            <input type="url" className="w-full bg-white/3 border border-white/10 p-5 outline-none focus:border-accent text-lg text-white italic font-black"
                                   value={raceForm.imageUrl}
                                   onChange={(e)=>setRaceForm({...raceForm, imageUrl: e.target.value})}
                            />
                        </div>
                        <div className="md:col-span-4 space-y-2">
                            <label className="text-[10px] text-white/40 tracking-widest ml-1 uppercase text-xs">
                                Race Briefing {raceForm.description.trim() === "" && <span className="text-red-500 animate-pulse ml-2">(Required)</span>}
                            </label>
                            <textarea rows={3} className="w-full bg-white/3 border border-white/10 p-4 outline-none focus:border-accent transition-all text-sm italic font-black text-white resize-none"
                                      value={raceForm.description}
                                      onChange={(e)=>setRaceForm({...raceForm, description: e.target.value})}
                            />
                        </div>
                    </section>

                    <section className="bg-white/2 border border-white/5 p-6 rounded-sm text-white">
                        <h3 className="text-[10px] tracking-[0.4em] mb-6 text-green-500/60 font-black uppercase text-xs italic">Quick Add New Obstacle To System</h3>
                        <div className="grid grid-cols-1 md:grid-cols-4 gap-4 items-end">
                            <input
                                value={newObstacle.name}
                                onChange={(e)=>setNewObstacle({...newObstacle, name: e.target.value})}
                                placeholder="Obstacle Name" className="bg-dark border border-white/10 p-4 text-xs outline-none focus:border-green-500 transition-all uppercase font-black text-white italic" />
                            <input
                                value={newObstacle.description}
                                onChange={(e)=> setNewObstacle({...newObstacle, description: e.target.value})}
                                placeholder="Short Description" className="bg-dark border border-white/10 p-4 text-xs outline-none focus:border-green-500 transition-all uppercase font-black text-white italic" />
                            <select
                                value={newObstacle.difficulty}
                                onChange={(e) => setNewObstacle({...newObstacle, difficulty: Number(e.target.value)})}
                                className="bg-dark border border-white/10 p-4 text-xs outline-none cursor-pointer uppercase font-black text-white italic">
                                <option value="0">Easy</option>
                                <option value="1">Normal</option>
                                <option value="2">Hard</option>
                            </select>
                            <button
                                onClick={async () => {
                                    try {
                                        setIsLoading(true);
                                        const created = await obstacleService.createObstacle(newObstacle);
                                        setAllObstacles(prev => [...prev, created]);
                                        setRaceForm(prev => ({ ...prev, obstacleIds: [...prev.obstacleIds, created.id] }));
                                        setNewObstacle({ name: '', description: '', difficulty: 0 });
                                    } catch (e) { console.error(e); } finally { setIsLoading(false); }
                                }}
                                disabled={isLoading || !isCreateObstacleValid}
                                className={!isCreateObstacleValid?"opacity-50 cursor-not-allowed bg-green-600/10 text-green-500 border border-green-500/30 p-4 text-[11px] hover:bg-green-600 hover:text-white transition-all font-black uppercase shadow-lg":"bg-green-600/10 text-green-500 border border-green-500/30 p-4 text-[11px] hover:bg-green-600 hover:text-white transition-all cursor-pointer font-black uppercase shadow-lg"}>
                                {isLoading ? "DEPLOYING..." : "CREATE & ADD"}
                            </button>
                        </div>
                    </section>

                    <section className="space-y-6">
                        <div className="flex flex-col md:flex-row justify-between items-end md:items-center gap-4">
                            <h3 className="tracking-tighter italic font-black uppercase text-3xl">Active Obstacle Mapping</h3>
                            <div className="relative w-full md:w-80">
                                <input type="text" placeholder="Filter library..." className="w-full bg-white/3 border border-white/10 p-4 pl-10 text-[10px] outline-none focus:border-accent italic font-black text-white" />
                                <span className="absolute left-4 top-1/2 -translate-y-1/2 text-white/20 italic tracking-tighter uppercase font-black">🔍</span>
                            </div>
                        </div>

                        <div className="border border-white/10 rounded-sm overflow-hidden bg-white/1 shadow-xl text-white">
                            <div className="max-h-80 overflow-y-auto no-scrollbar scroll-smooth">
                                <table className="w-full text-left text-[11px] border-collapse uppercase text-white font-black italic">
                                    <thead className="bg-[#1a2421] border-b border-white/10 uppercase tracking-widest sticky top-0 z-10 text-white">
                                    <tr>
                                        <th className="p-6 w-16 text-center">LINK</th>
                                        <th className="p-6 text-white">Obstacle Name</th>
                                        <th className="p-6 hidden md:table-cell text-white">Description</th>
                                        <th className="p-6 text-center text-white">Difficulty</th>
                                        <th className="p-6 text-right text-white">System Action</th>
                                    </tr>
                                    </thead>
                                    <tbody className="divide-y divide-white/5 italic font-black text-sm text-white">
                                    {allObstacles.map((obstacle) => (
                                        <tr key={obstacle.id} className="hover:bg-white/4 transition-all border-b border-white/5 group">
                                            <td className="p-6 text-center">
                                                <input type="checkbox" className="accent-accent w-6 h-6 cursor-pointer opacity-40 checked:opacity-100 text-white"
                                                       checked={raceForm.obstacleIds.includes(obstacle.id)}
                                                       onChange={() => handleObstacleToggle(obstacle.id)}
                                                />
                                            </td>
                                            <td className="p-6 font-black text-white/90 text-base italic uppercase">{obstacle.name}</td>
                                            <td className="p-6 text-white/40 not-italic hidden md:table-cell normal-case font-medium text-xs">{obstacle.description}</td>
                                            <td className="p-6 text-center">
                                                <span className={`px-4 py-2 border text-[11px] font-black tracking-widest uppercase italic ${
                                                    obstacle.difficulty === 2 ? 'bg-red-500/10 text-red-500 border-red-500/20' :
                                                        obstacle.difficulty === 1 ? 'bg-orange-500/10 text-orange-500 border-orange-500/20' :
                                                            'bg-green-500/10 text-green-500 border-green-500/20'
                                                }`}>{Difficulty[obstacle.difficulty]}
                                                </span>
                                            </td>
                                            <td className="p-6 text-right text-white">
                                                <button onClick={() => { setItemToDelete(obstacle.id); setIsObstacleDeleteModalOpen(true); }} className="text-red-500/40 hover:text-red-500 cursor-pointer transition-all text-[11px] font-black uppercase italic">
                                                    [ REMOVE ]
                                                </button>
                                            </td>
                                        </tr>
                                    ))}
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </section>
                    <div className="min-h-12.5 flex items-end">
                        {error && (
                            <div className="w-full bg-red-500/10 border border-red-500/50 p-4 animate-pulse">
                                <p className="text-red-500 text-xs font-black tracking-widest uppercase">
                                    [ ERROR ]: {error}
                                </p>
                            </div>
                        )}
                    </div>
                    <div className="pt-8 border-t border-white/5 space-y-8 text-white">
                        <div className="flex flex-col md:flex-row gap-4">
                            <button onClick={() => {setIsDeleteModalOpen(true); setItemToDelete(id)}} className="md:w-1/4 bg-red-600/10 text-red-500 border border-red-500/30 py-6 text-xl font-black hover:bg-red-600 hover:text-white transition-all cursor-pointer uppercase italic flex items-center justify-center tracking-tighter shadow-lg">
                                [ DELETE RACE ]
                            </button>
                            <button
                                onClick={updateRace}
                                className={!isFormValid
                                    ? "md:w-3/4 bg-white/5 text-white/20 py-6 text-3xl font-black transition-all uppercase italic tracking-tighter cursor-default shadow-lg"
                                    : "md:w-3/4 bg-accent text-dark py-6 text-3xl font-black hover:bg-accent/90 transition-all cursor-pointer uppercase italic tracking-tighter transform active:scale-[0.98] shadow-lg"}
                            >
                                {isLoading ? "DEPLOYING..." : !isFormValid ? "FORM INCOMPLETE" : "UPDATE RACE"}
                            </button>
                        </div>
                        <p className="text-center text-[9px] text-white/10 tracking-[0.5em] font-medium uppercase italic">Security Protocol: Modifications affect all active registration data linked to this archive.</p>
                    </div>
                </div>
            </div>

            <ConfirmModal
                isOpen={isDeleteModalOpen}
                onClose={() => setIsDeleteModalOpen(false)}
                onConfirm={deleteRace}
                title="TERMINATE ARCHIVE"
                message="ARE YOU SURE? THIS WILL PERMANENTLY DELETE THIS RACE AND ALL ASSOCIATED REGISTRATIONS. THIS ACTION CANNOT BE REVERTED."
                error={error}
                variant={"danger"}
            />

            <ConfirmModal
                isOpen={isObstacleDeleteModalOpen}
                onClose={() => setIsObstacleDeleteModalOpen(false)}
                onConfirm={deleteObstacle}
                title="DELETE OBSTACLE"
                message="YOU ARE ABOUT TO PERMANENTLY DELETE THIS OBSTACLE FROM THE SYSTEM ARCHIVE. IT WILL BE REMOVED FROM ALL RACES."
                error={error}
                variant={"danger"}
            />
        </div>
    );
}
export default EditRace;